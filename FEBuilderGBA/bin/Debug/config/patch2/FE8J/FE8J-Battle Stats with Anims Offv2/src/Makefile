
.SUFFIXES:
.PHONY:

# Making sure devkitARM exists and is set up.
ifeq ($(strip $(DEVKITARM)),)
	$(error "Please set DEVKITARM in your environment. export DEVKITARM=<path to>devkitARM")
endif

# Including devkitARM tool definitions
include $(DEVKITARM)/base_tools
# defining path of lyn
LYN := "C:/devkitPro/lyn.exe"

# setting up compilation flags
ARCH   := -mcpu=arm7tdmi -mthumb -mthumb-interwork
CFLAGS := $(ARCH) -Wall -mtune=arm7tdmi -mlong-calls -O3

# C to ASM rule
%.s: %.c *.c
	$(CC) $(CFLAGS) -S $< -I C:/devkitPro/FE-CLib/include -o $@ -fverbose-asm -mlong-calls


# OBJ to EVENT rule
%.lyn.event: %.o *.c
	$(MAKE) $(dir $<)Definitions.o
	$(LYN) "$<" "$(dir $<)Definitions.o" > "$@"
	rm $(dir $<)Definitions.o
	
# ASM to OBJ
%.o: %.s *.s
	$(AS) $(ARCH) $< -o $@
	