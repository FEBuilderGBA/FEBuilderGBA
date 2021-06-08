import image

image.make_font_EA('bold_text', 'FE8_Text_Bold.png', 4)
#For the italic font, I set the kerning to 0 (line 315 of image.py)
#I commented this line out so that adding another font wouldn't overwrite the (somewhat nice) italic.
#image.make_font_EA('ital_text', 'FE8_Text_Ital.png', 4)

###if adding new fonts, stick 'em in here
##fonts_list = ['FE8_Text_Bold.png', 'FE8_Text_Ital.png']
##
##with open('Master_Font_Installer.txt','w') as f:
##    for font in fonts_list:
##        image.make_font_EA(font[:-4], font, 4)
##        f.write('#include {}.txt\n'.format(font[:-4]))
