"""
Copyright 2021 Zahlman

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
"""

from collections.abc import Sequence
import itertools, struct, zlib


def make_row(row_bytes, source, i):
    if source is None:
        return bytearray(row_bytes)
    result = bytearray(source[i])
    result.extend([0] * (row_bytes - len(result)))
    return result[:row_bytes]


def mask(original, replacement):
    return original if replacement == 0 else replacement


class Image2bpp(Sequence):
    """Represents a 2bpp image bitmap in a compact format."""
    def __init__(self, width, height, source=None):
        row_bytes = (width + 3) // 4
        self._width = width
        self._rows = [make_row(row_bytes, source, i) for i in range(height)]


    @staticmethod
    def from_bytes(data, offset, bytes_per_row, rows):
        return Image2bpp(bytes_per_row * 4, rows, source=[
            data[o:o+bytes_per_row]
            for o in range(offset, offset+rows*bytes_per_row, bytes_per_row)
        ])


    @property
    def width(self):
        return self._width


    @property
    def height(self):
        return len(self._rows)


    def __len__(self):
        return len(self._rows)


    def __getitem__(self, row):
        if isinstance(row, slice):
            raise TypeError('cannot slice')
        if not 0 <= row < self.height:
            raise IndexError('n.b. negative indices not allowed')
        return Image2bppRow(self._rows[row], self._width)


    def tile(self, x, y):
        # extract a 16x16 region (16 rows of 4 bytes each)
        return [self._rows[y*16+j][x*4:x*4+4] for j in range(16)]


    def copy(self):
        return Image2bpp(self.width, self.height, self._rows)


    def blit(self, source, x, y, op=mask):
        if source is self:
            source = self.copy()
        for j in range(max(0, -y), min(source.height, self.height - y)):
            for i in range(max(0, -x), min(source.width, self.width - x)):
                self[y+j][x+i] = op(self[y+j][x+i], source[j][i])


    def __str__(self):
        return '\n'.join(str(row) for row in self)


class Image2bppRow(Sequence):
    def __init__(self, scanline, width):
        self._scanline = scanline
        self._width = width


    def __len__(self):
        return self._width


    def __getitem__(self, column):
        if isinstance(column, slice):
            raise TypeError('cannot slice')
        if not 0 <= column < self._width:
            raise IndexError('n.b. negative indices not allowed')
        byte, bits = divmod(column, 4)
        return (self._scanline[byte] >> (bits * 2)) & 3


    def __setitem__(self, column, value):
        if value not in (0, 1, 2, 3):
            raise ValueError('invalid sample value (must be 0..3)')
        byte, bits = divmod(column, 4)
        old = self._scanline[byte]
        mask = ~(3 << (bits * 2))
        self._scanline[byte] = (old & mask) | (value << (bits * 2))


    def __str__(self):
        return '[{}]'.format(''.join(' .o*'[sample] for sample in self))


def make_font_image(source, table_start):
    canvas = Image2bpp(256, 256)
    for i, offset in enumerate(range(table_start, table_start + 1024, 4)):
        glyph_offset = rom_pointer(source, offset) + 8 # glyph header
        y, x = divmod(i, 16)
        glyph = Image2bpp.from_bytes(source, glyph_offset, 4, 16)
        canvas.blit(glyph, x * 16, y * 16)
    return canvas


def rom_pointer(source, offset):
    return struct.unpack_from('<I', source, offset)[0] - 0x08000000


def as_quad(value):
    return struct.pack('>I', value)


def make_header(img):
    return struct.pack('>2I5B', img.width, img.height, 2, 3, 0, 0, 0)


def make_rows(img):
    result = bytearray()
    for row in img:
        result.append(0)
        value = 0
        for sample, position in zip(row, itertools.cycle(range(6, -2, -2))):
            value |= sample << position
            if position == 0:
                result.append(value)
                value = 0
        if len(row) % 4:
            result.append(value)
    return zlib.compress(result)


def append_png_chunk(output, data, name):
    output.write(as_quad(len(data)))
    chunk = name + data
    output.write(chunk)
    output.write(as_quad(zlib.crc32(chunk)))


def make_png(filename, img):
    png_signature = b'\x89\x50\x4e\x47\x0d\x0a\x1a\x0a'
    basic_palette = b'\xf8\xf8\xf8\xd8\xd8\xd8\xb8\xb8\xb8\x28\x28\x28'
    with open(filename, 'wb') as output:
        output.write(png_signature)
        append_png_chunk(output, make_header(img), b'IHDR')
        append_png_chunk(output, basic_palette, b'PLTE')
        append_png_chunk(output, make_rows(img), b'IDAT')
        append_png_chunk(output, b'', b'IEND')


def make_font_pngs(source_filename, *offsets):
    with open(source_filename, 'rb') as f:
        data = f.read()
    for offset in offsets:
        make_png('{:6x}.png'.format(offset), make_font_image(data, offset))


# reading


def quad_to_int(quad):
    return struct.unpack('>I', quad)[0]


def get_quad(stream, required=True):
    result = stream.read(4)
    if not (required or result):
        return None
    if len(result) != 4:
        raise ValueError('unexpected EOF')
    return result


def png_chunk_gen(stream):
    header = stream.read(8)
    if header != b'\x89PNG\r\n\x1a\n':
        raise ValueError('PNG header not found')
    while True:
        size = get_quad(stream, False)
        if size is None:
            break
        kind = get_quad(stream)
        size = quad_to_int(size)
        data = stream.read(size)
        get_quad(stream)
        yield (kind, data)


def png_chunks(filename):
    result = {}
    with open(filename, 'rb') as f:
        for k, v in png_chunk_gen(f):
            if k in result:
                if k == b'IDAT':
                    result[b'IDAT'] += v
                else:
                    raise ValueError('Duplicate {} chunk'.format(repr(k)))
            else:
                result[k] = v
        return result


def process_scanline(ftype, current, previous):
    left = 0
    for p, c, d in zip(previous, current, bytes(1) + previous[:-1]):
        l = left
        if l > 0x7f:
            l -= 0x100
        if ftype == 0:
            prediction = 0
        elif ftype == 1: # sub
            prediction = l
        elif ftype == 2: # up
            prediction = p
        elif ftype == 3: # average filter
            prediction = (l + p) // 2
        elif ftype == 4: # paeth
            initial = l + p - d
            prediction = min((l, p, d), key = lambda x: abs(initial - x))
        else:
            raise ValueError('Invalid filter type detected')
        left = (c + prediction) & 0xff
        yield left


def sanity_checks(chunks):
    try:
        IHDR = chunks[b'IHDR']
    except KeyError:
        raise ValueError('Missing IHDR chunk')
    width, height, bitdepth, ctype, x, y, z = struct.unpack('>2I5B', IHDR)
    if x != 0:
        raise ValueError('Unrecognized PNG compression method')
    if y != 0:
        raise ValueError('Unrecognized PNG filter method')
    if z == 1:
        raise ValueError('Interlaced PNGs currently unsupported')
    elif z != 0:
        raise ValueError('Unrecognized PNG interlacing method')
    if ctype != 3:
        raise ValueError('Please use a palettized PNG')
    if bitdepth != 8:
        raise ValueError('Please use a 256-colour palette')
    try:
        PLTE = chunks[b'PLTE']
    except KeyError:
        raise ValueError('Missing PLTE chunk')
    palette_size, remainder = divmod(len(PLTE), 3)
    if remainder:
        raise ValueError('PLTE chunk has invalid length')
    try:
        IDAT = chunks[b'IDAT']
    except:
        raise ValueError('Missing IDAT')
    else:
        return IDAT, width, height


def process_scanlines(IDAT, width, height):
    result = Image2bpp(width, height)
    data = zlib.decompress(IDAT)
    bytes_per_line = width + 1
    previous_scanline = b'\x00' * width
    for i in range(height):
        base = bytes_per_line * i
        previous_scanline = bytes(process_scanline(
            data[base], data[base+1:base+bytes_per_line],
            previous_scanline
        ))
        for j, b in enumerate(previous_scanline):
            result[i][j] = b
    return result


def image_from_file(filename):
    return process_scanlines(*sanity_checks(png_chunks(filename)))


def wl_helper(b):
    for mask, result in zip((0xC0, 0x30, 0x0C, 0x03), (4, 3, 2, 1)):
        if b & mask:
            return result
    assert False


def width_of_line(line):
    a, b, c, d = line
    # 4 px per byte to the left of the one examined
    for offset, candidate in zip((12, 8, 4, 0), (d, c, b, a)):
        if candidate != 0:
            return offset + wl_helper(candidate)
    # Empty line.
    return 0


def glyph(img, number, space_width):
    row, column = divmod(number, 16)
    raw = img.tile(column, row)
    width = max(map(width_of_line, raw))
    if width == 0:
        width = space_width
    else:
        width += 1 # kerning.
    return bytes([0, 0, 0, 0, 0, width, 0, 0]) + b''.join(raw)
#    return str('0 0 0 0 0 {} 0 0 {}'.format(hex(width),(raw)))

def hexdump_glyph(b):
    assert len(b) == 72
    line = ' '.join(['{:02X}']*8)
    return [None] + [
        line.format(*b[i*8:i*8+8])
        for i in range(9)
    ]

def glyph_to_ea(glyph_output):
    assert len(glyph_output) == 72
    return ('BYTE' + (' 0x{:02X}' * 72)).format(*glyph_output)
	
def make_font(name, filename, space_width):
    img = image_from_file(filename)
    glyphs = {} # mapping from a glyph to a unique identifier
    table = [None]
    for i in range(256):
        g = glyph(img, i, space_width if i == 32 else 0)
        if g not in glyphs:
            glyphs[g] = '{}_{}'.format(name, i)
        table.append({'referent': glyphs[g]})
    result = {v: hexdump_glyph(k) for k, v in glyphs.items()}
    result['{}_font'.format(name)] = table
    return result

def make_font_EA(name, filename, space_width):
    #creates an EA installer for this font
    img = image_from_file(filename)
    with open('{}.txt'.format(str(filename[:-4])),'w') as f: #the slice is to remove the .png
        glyph_count = 256 #for testing purposes (normally will be 256)
        f.write('ALIGN 4\nPOIN ')
        #generates the pointer table to each glyph
        for i in range(glyph_count):
            f.write('{} '.format(name+str(i)))
        f.write('\n')
        #actually make the graphics data
        for j in range(glyph_count):
            f.write('\n{}:\n'.format(name+str(j)))
            g = glyph(img, j, space_width if j == 32 else 0)
            f.write(glyph_to_ea(g))