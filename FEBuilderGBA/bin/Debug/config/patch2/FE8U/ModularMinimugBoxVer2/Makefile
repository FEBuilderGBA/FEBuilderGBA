
SHELL = /bin/sh

.PHONY:
.SUFFIXES:

# Shoutout to Stan for Makefile wizardry,
# tools, and being a cool person.

# Making sure devkitARM exists and is set up
ifeq ($(strip $(DEVKITARM)),)
  $(error "Please set DEVKITARM in your environment. export DEVKITARM=<path to>devkitARM")
endif

# Including devkitARM tool definitions
include $(DEVKITARM)/base_tools

ifeq ($(OS),Windows_NT)
  EXE := .exe
else
  EXE :=
endif

# Additional tools

export PNG2DMP := $(realpath .)/Tools/EventAssembler/Tools/Png2Dmp$(EXE)
export EADEP   := $(realpath .)/Tools/ea-dep$(EXE)
export LYN     := $(realpath .)/Tools/lyn$(EXE)

EVENT_DEPENDS := $(shell $(EADEP) "MMBCore.event" -I $(realpath .)/Tools/EventAssembler --add-missings)

ASFLAGS := -g -mcpu=arm7tdmi -mthumb -mthumb-interwork

# Recipes

.PHONY: all clean

all: $(EVENT_DEPENDS)

# Object -> lyn

%.lyn.event: %.o Internal/Definitions.o
	@$(LYN) $< Internal/Definitions.o > $@

# Assembly source -> Object

%.o: %.s
	@$(AS) $(ASFLAGS) -I $(dir $<) $< -o $@

# Images

%.4bpp: %.png
	@$(PNG2DMP) $< -o $@

clean:
	@$(RM) Internal/*.lyn.event
	@$(RM) Internal/*.o
	@$(RM) Modules/*.lyn.event
	@$(RM) Modules/*.o
	@$(RM) Modules/*.4bpp
