@echo off

set StartDir=C:\devkitPro\devkitARM\arm-none-eabi\bin\
set IncludeDir=%~dp0_CommonASM

set as="%StartDir%as"
set nm="%StartDir%nm"
set objcopy="%StartDir%objcopy"

@rem Assemble into an elf
%as% -g -mcpu=arm7tdmi -mthumb-interwork -I "%IncludeDir%" %1 -o "%~n1.elf"

(%nm% -u "%~n1.elf") > _____temp.txt
set /p _undefinedSymbols= < _____temp.txt
echo y | del _____temp.txt

if ["%_undefinedSymbols%"] equ [""] (
	%objcopy% -S "%~n1.elf" -O binary "%~n1.bin"
) else (
    echo ERROR: Found some undefined symbols: >&2
    echo %_undefinedSymbols% >&2
)

echo y | del "%~n1.elf"
pause
