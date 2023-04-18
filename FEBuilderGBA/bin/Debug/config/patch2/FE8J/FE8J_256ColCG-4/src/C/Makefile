.SUFFIXES:
.PHONY: all clean

# making sure devkitARM exists and is set up
ifeq ($(strip $(DEVKITARM)),)
	$(error "Please set DEVKITARM in your environment. export DEVKITARM=<path to>devkitARM")
endif

# including devkitARM tool definitions
include $(DEVKITARM)/base_tools

# defining path of lyn
LYN := $ C:/devkitPro/lyn.exe

# Setting C/ASM include directories up (there is none yet)
INCLUDE_DIRS := C:/FE-CLib/include
INCFLAGS     := $(foreach dir, $(INCLUDE_DIRS), -I "$(dir)")

# setting up compilation flags
ARCH   := -mcpu=arm7tdmi -mthumb -mthumb-interwork
CFLAGS := $(ARCH)$(INCFLAGS) -Wall -Os -mtune=arm7tdmi -mlong-calls

# lyn reference object
LYNREF := C:/FE-CLib/reference/FE8U.o

# listing files
CFILES := $(wildcard *.c)
SFILES := $(wildcard *.asm)
OFILES := $(CFILES:.c=.o) $(SFILES:.asm=.o)
ASMFILES := $(CFILES:.c=.s)
LYNFILES := $(OFILES:.o=.lyn.event)
DMPFILES := $(OFILES:.o=.dmp)

# defining & making dependency directory
DEPSDIR := .MkDeps
$(shell mkdir -p $(DEPSDIR) > /dev/null)

# listing possible dependency files
DEPFILES := $(addprefix $(DEPSDIR)/, $(notdir $(CFILES:.c=.d)))

# dependency generation flags for CC
CDEPFLAGS = -MD -MT $*.o -MT $*.s -MF $(DEPSDIR)/$(notdir $*).d -MP

# "make all" phony targets
all: $(OFILES) $(ASMFILES) $(LYNFILES) $(DMPFILES);

# "Clean" target
clean:
	rm -f $(OFILES) $(ASMFILES) $(LYNFILES) $(DMPFILES) $(DEPFILES)

# C to ASM rule
%.s: %.c
	$(CC) $(CFLAGS) $(CDEPFLAGS) -S $< -o $@

# C to OBJ rule
%.o: %.c
	$(CC) $(CFLAGS) $(CDEPFLAGS) -c $< -o $@

# ASM to OBJ rule
%.o: %.asm
	$(AS) $(ARCH) $< -o $@

# OBJ to DMP rule
%.dmp: %.o
	$(OBJCOPY) -S $< -O binary $@

# OBJ to EVENT rule
%.lyn.event: %.o $(LYNREF)
	$(LYN) $< $(LYNREF) > $@

# Including generated dependency rules
-include $(DEPFILES)