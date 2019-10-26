#include "include\gbafe.h"

// AutoNewline. Hack by Zeta/Gilgamesh. Original concept by laqieer ( https://github.com/laqieer/FE7CNLOL/blob/master/src/FE7CNText.c )
// Requires FE-CLIB
// Free to use/modify

// hook at $464476

//void GetCharTextWidthAscii(char* character, u8* returnValue); //FE8U 0x8004539

//#define DEBUG

#define TEXT_X					0x00
#define TEXT_N					0x01
#define TEXT_CLEAR				0x02
#define TEXT_A					0x03
#define TEXT_PAUSE				0x04
#define TEXT_LOADFACE 			0x10
#define TEXT_CLEARFACE			0x11
#define TEXT_CLOSESPEECHFAST	0x14
#define TEXT_CLOSESPEECHSLOW	0x15
#define TEXT_TOGGLEMOUTHMOVE	0x16

#define TEXT_SHORTPAUSE			0x1F

extern u8 Text_Conversation;
extern u8 Text_Battle;
extern u8 Text_Misc;

extern u8 MaxConversationWidth;
extern u8 MaxBattleWidth;
extern u8 MaxMiscWidth;
extern u8 PunctuationLinebreak;
extern u8 BreakEarlyOnLineTwo;
extern u8 MinimumSentenceWidth;
extern u8 TryKeepSentencesTogether;
extern u8 BeatPause;
extern u8 PunctuationPause;
extern char* RAMLocation;

#ifdef DEBUG
// yeah I'm sure this could be done more intelligently
// but I just needed a quick debug function so whatever
u8 IntToString(int Source, char* Destination)
{
	if (Source < 10)
	{
		Destination[0] = Source + 0x30;
		return 1;
	}
	else if (Source >= 10 && Source <= 99)
	{
		int firstDigit = Div(Source, 10);
		int secondDigit = Source - (firstDigit * 10);
		
		Destination[0] = firstDigit + 0x30;
		Destination[1] = secondDigit + 0x30;
		return 2;
	}
	else if (Source >= 99 && Source <= 999)
	{
		int firstDigit = Div(Source, 100);
		int secondDigit = Div(Source - (firstDigit * 100), 10);
		int thirdDigit = Source - (firstDigit * 100) - (secondDigit * 10);
		
		Destination[0] = firstDigit + 0x30;
		Destination[1] = secondDigit + 0x30;
		Destination[2] = thirdDigit + 0x30;
		return 3;
	}
	else
	{
		Destination[0] = '?';
		Destination[1] = '?';
		return 2;
	}
}
#endif

bool IsValidASCII(const char source)
{
	return (source >= 0x20 && source <= 0x7F);
}

bool IsOpenLocation(const char source)
{
	return (source >= 0x08 && source <= 0x0F);
}

bool IsTextStartCode(const char source)
{
	return (source == Text_Conversation || source == Text_Battle || source == Text_Misc);
}

bool IsPrintable(const char* source)
{
	return (IsValidASCII(source[0]) && source[-1] != TEXT_LOADFACE);
}

char* StringCopy(const char* Source, char* Destination)
{
	int x = 0;
	while (Source[x] != 0x0)
	{
		Destination[x] = Source[x];
		x++;
	}
	Destination[x] = 0x0; // null terminator
	
	return &Destination[x];
}

u8 IsLinebreakPunctuation(char source)
{
	if (source == '.' || source == '!' || source == '?' || source == '!')
		return 1;
	else
		return 0;
}

char NextPrintable(const char* source)
{
	char output = 0;
	
	for (int x = 1; source[x] != TEXT_X && !output; x++)
	{
		if (IsPrintable(&source[x]))
			output = source[x];
	}
	
	return output;
}

char LastPrintable(const char* source)
{
	char output = 0;
	
	for (int x = -1; !IsTextStartCode(source[x]) && !output; x--)
	{
		if (IsPrintable(&source[x]))
			output = source[x];
	}
	
	return output;
}

int GetNextSentenceWidth(const char* source)
{
	int width = 0;
	int sentenceWidth = 0;
	unsigned temp = 0;
	
	for (int x = 1; source[x] != TEXT_A && source[x] != TEXT_X; x++)
	{
		if (IsPrintable(&source[x]))
		{
			if (IsLinebreakPunctuation(LastPrintable(&source[x])) && !IsLinebreakPunctuation(source[x]))
			{
				if (sentenceWidth > MinimumSentenceWidth)
				{
					break;
				}
				else
				{
					sentenceWidth = 0;
				}
			}
			
			GetCharTextWidthAscii(&source[x], &temp);
			width += temp;
			sentenceWidth += temp;
		}
	}
	
	return width;
}

int GetLastSentenceWidth(const char* source)
{
	int width = 0;
	unsigned temp = 0;
	
	for (int x = -1; source[x] != TEXT_A && source[x] != TEXT_X; x--)
	{
		if (IsPrintable(&source[x]))
		{
			if (IsLinebreakPunctuation(LastPrintable(&source[x])) && !IsLinebreakPunctuation(source[x]))
			{
				break;
			}
			
			GetCharTextWidthAscii(&source[x], &temp);
			width += temp;
		}
	}
	
	return width;
}

/*
int GetNextSentenceWidth(const char* source)
{
	int width = 0;
	unsigned temp = 0;
	
	for (int x = 0; !IsLinebreakPunctuation(source[x]); x++)
	{
		if (IsValidASCII(source[x]))
		{
			GetCharTextWidthAscii(&source[x], &temp);
			width += temp;
		}
	}
	
	return width;
}
*/


bool DidWeAlreadyLinebreak(const char* source)
{
	for (int x = -1; !IsTextStartCode(source[x]); x--)
	{
		// if we find a printable character code before we find [A] or [N] we're good.
		if (IsPrintable(&source[x]))
			return false;
		if (source[x] == TEXT_A || source[x] == TEXT_N)
			return true;
	}
	
	return false;
}

bool WillWeLinebreakSoon(const char* source)
{
	for (int x = 1; source[x] != TEXT_X; x++)
	{
		// if we find a printable character code before we find [A] or [N] we're good.
		if (IsPrintable(&source[x]))
			return false;
		if (source[x] == TEXT_A || source[x] == TEXT_N)
			return true;
	}
	
	return false;
}

bool IsTextEnd(const char* source)
{
	for (int x = 0; source[x] != TEXT_X; x++)
	{
		if (IsPrintable(&source[x]))
			return false;
	}
	
	return true;
}

// XXX: the modulus operator doesn't seem to actually work
// and Mod() == undefined opcode
bool HackyTemporarySolution(int periodCount)
{
	if (periodCount == 0)
		return false;
	
	while (periodCount > 0)
		periodCount -= 3;
	
	if (periodCount == 0)
		return true;
	else
		return false;
}



// this function handles adding [A][N] and spaces where necessary in response to control codes.
const char* PreprocessString(const char* source, bool miscText)
{
	char *destination = RAMLocation;
	int x = 0;
	int y = 0;	
	bool midLine = false;
	
	if (miscText)
	{
		StringCopy(source, destination);
		return destination;
	}
	
	while (source[x] != TEXT_X)
	{
		// this loop handles printable characters in specific
		// less of a clusterfuck to have seperate loops for each.
		while (IsPrintable(&source[x]))
		{
			midLine = true;
			
			if (IsLinebreakPunctuation(source[x]) || source[x] == ',')
			{
				bool alreadyPaused = false;
				int periodCount = 0;
				for (int x2 = x; source[x2] == '.'; x2++)
					periodCount++;
				
				// so we need to handle "Some Text.... Some more text." by inserting a space after the first ... (because it's almost certainly was "Some Text.\n... Some more text" pre textprocess)
				// at the same time we need to handle "Some Text...?" by not inserting a space until it's over.
				// so we explicitly check for four periods in a row
				if (periodCount == 4)
				{
					destination[y++] = source[x++]; // copy the period
					destination[y++] = ' ';
					
					periodCount--; // because we'll always want to trigger the beat handling.
				}
				
				while (HackyTemporarySolution(periodCount)) // supposed to be periodCount % 3 == 0, but the modulus operator doesn't seem to work.
				{
					destination[y++] = TEXT_TOGGLEMOUTHMOVE;
					destination[y++] = source[x++];
					destination[y++] = source[x++];
					destination[y++] = source[x++];
					while (IsLinebreakPunctuation(source[x]) && source[x] != '.')
						destination[y++] = source[x++]; // copy the three periods and any other linebreak punctuation (so this should handle inanity like "...?!"
					if (BeatPause)
						destination[y++] = BeatPause;
					destination[y++] = TEXT_TOGGLEMOUTHMOVE;
					
					periodCount -= 3;
					
					if (periodCount > 0)
						destination[y++] = ' ';
					
					alreadyPaused = true;
				}
				
				// now we can just copy punctuation over
				while (IsLinebreakPunctuation(source[x]) || source[x] == ',')
				{
					destination[y++] = source[x++];
				}
					
				if (PunctuationPause && !alreadyPaused)
				{
					destination[y++] = PunctuationPause;
				}
			}
			else if (source[x] == '-')
			{
				destination[y++] = TEXT_TOGGLEMOUTHMOVE;
				while (source[x] == '-')
					destination[y++] = source[x++];
				destination[y++] = TEXT_TOGGLEMOUTHMOVE;
			}
			else // no printable other than !?.,- currently needs special handling, so just chuck them onwards.
			{
				destination[y++] = source[x++];
			}
		}
		
		// now we handle non printable text codes
		while (!IsPrintable(&source[x]) && source[x] != TEXT_X)
		{	
			// none of these codes should be in misc text in the first place, so no need to check
			if (source[x] == TEXT_LOADFACE) // handle [LoadFace]
			{
				destination[y++] = source[x++];
				destination[y++] = source[x++];
				destination[y++] = source[x++];
			}
			else if (midLine && (IsOpenLocation(source[x]) || source[x] == TEXT_CLEARFACE || source[x] == TEXT_CLOSESPEECHFAST || source[x] == TEXT_CLOSESPEECHSLOW)) // handle various codes that require line breaks before them.
			{
				destination[y++] = TEXT_A;		
				destination[y++] = TEXT_N;
				destination[y++] = source[x++];
				
				midLine = false;
			}
			else if (source[x] == 0x80) // handle the two byte codes
			{
				if (source[x + 1] == 0x04 && midLine) // handle [LoadOverworldFaces]
				{
					destination[y++] = TEXT_A;		
					destination[y++] = TEXT_N;
						
					midLine = false;
				}
				
				destination[y++] = source[x++]; 
				destination[y++] = source[x++];
			}
			else // unknown code, just copy it.
			{
				destination[y++] = source[x++];
			}
		}
	}
	if (midLine)
		destination[y++] = TEXT_A;
	destination[y++] = TEXT_X;
	
	return destination;
}

void HandleAutoNewline(const char* input, char* destination)
{
	const u8 textType = input[0];
	input++; // skip past the textType byte
	int maxWidth = 0;
	
	if (textType == Text_Conversation)
		maxWidth = MaxConversationWidth;
	else if (textType == Text_Battle)
		maxWidth = MaxBattleWidth;
	else
		maxWidth = MaxMiscWidth;
	
	bool miscText = textType == Text_Misc ? 1 : 0;
	
	const char* source = PreprocessString(input, miscText);
	
	int x = 0;
	int y = 0;
	unsigned currentWidth = 0;
	unsigned temp = 0;
	bool lineTwo = false;
	bool insertLineBreak = false;
	bool wePressedA = false;
	bool weLinebroke = false;
	bool weCleared = false;
	
	int lastSpaceSource = 0;
	int lastSpaceDestination = 0;
	
	// XXX: this should probably be a constant instead
	const char aSpace = ' ';
	unsigned spaceWidth = 0;
	GetCharTextWidthAscii(&aSpace, &spaceWidth);
	
	
	while (source[x] != TEXT_X)
	{
		while (!IsPrintable(&source[x]))
		{
			if ((source[x] == TEXT_A || source[x] == TEXT_N || source[x] == TEXT_CLEAR) && source[x - 2] != TEXT_LOADFACE && source[x - 1] != TEXT_LOADFACE) // loadface is [0x10][0xwhatever][0x01], so...
			{
				// sanity checks
				if ((wePressedA && source[x] == TEXT_A) || ((weLinebroke || weCleared) && source[x] == TEXT_N)) // multiple [A] or [N] in the same non printable string. skip it.
				{
					x++;
				}
				else if (currentWidth == 0) // [A] or [N] before printable characters??? skip it.
				{
					x++;
				}
				else
				{
					if (source[x] == TEXT_A)
					{
						wePressedA = true;
					}
					else if(source[x] == TEXT_N)
					{
						weLinebroke = true;
						
						if (lineTwo && !wePressedA && !miscText) // two [N]s without an [A] is a text skip and text skips are bad. so put the [A] in.
						{
							destination[y++] = TEXT_A;
						}
					}
					else if (source[x] == TEXT_CLEAR)
					{
						destination[y++] = TEXT_A;
						destination[y++] = TEXT_CLEAR;
						destination[y++] = TEXT_N;
						weCleared = true;
						weLinebroke = true;
						wePressedA = true;
					}
					
					destination[y++] = source[x++];
				}
			}
			else if (source[x] == TEXT_X) // if we hit the 'end of text' code, we're just outta here
			{
				destination[y++] = TEXT_X;
				return;
			}
			else
			{
				destination[y++] = source[x++];
			}
		}
		
		
		GetCharTextWidthAscii(&source[x], &temp);
		char lastPrintable = LastPrintable(&destination[y]);
		bool willWeLinebreakSoon = WillWeLinebreakSoon(&source[x]);
		if (IsTextEnd(&source[x + 1]))
		{
			insertLineBreak = false; // don't bother doing any more special handling. we're done here.
		}
		else if (weLinebroke || wePressedA)
		{
			if (wePressedA)
				lineTwo = false;
			else
				lineTwo = true;
			
			// skip spaces at the start of new lines
			while (source[x] == ' ')
				x++;
			
			currentWidth = 0;
			insertLineBreak = false;
			lastSpaceSource = 0;
			lastSpaceDestination = 0;
			
			weLinebroke = false;
			wePressedA = false;
		}
		else if (currentWidth + temp > maxWidth)
		{
			insertLineBreak = true;				
		}
		else if (IsLinebreakPunctuation(lastPrintable) && !IsLinebreakPunctuation(source[x]) && !willWeLinebreakSoon)
		{
			#ifdef DEBUG
			y += IntToString(GetNextSentenceWidth(&source[x]) + temp + (spaceWidth * 2) + currentWidth, &destination[y]);
			destination[y++] = ' ';
			//y += IntToString(GetLastSentenceWidth(&destination[y]), &destination[y]);
			//destination[y++] = ' ';
			#endif
			
			// multipliying spaceWidth because otherwise we risk underestimating the length of the next line (it may have a space automatically inserted as well)
			if (PunctuationLinebreak && !miscText && GetNextSentenceWidth(&source[x]) + temp + (spaceWidth * 2) + currentWidth >= maxWidth && GetLastSentenceWidth(&destination[y]) > MinimumSentenceWidth)
			{
				insertLineBreak = true;
				// we don't want to jump back to last space, just linebreak here
				lastSpaceSource = 0;
				lastSpaceDestination = 0;
			}
			else if (source[x] != ' ')
			{
				lastSpaceSource = x - 1;
				lastSpaceDestination = y;
				destination[y++] = ' ';
				currentWidth += spaceWidth;
			}			
		}
		else if (source[x] == ' ')
		{
			lastSpaceSource = x;
			lastSpaceDestination = y;
		}

		if (insertLineBreak) 
		{
			if (lastSpaceSource && lastSpaceDestination) 
			{
				x = lastSpaceSource;
				y = lastSpaceDestination;
				x++; // don't copy the space
			}

			if (!miscText)
			{
				if (lineTwo)
				{
					destination[y++] = TEXT_A;
					wePressedA = true;
				}
				else if (currentWidth + temp < maxWidth && TryKeepSentencesTogether) // if we line broke due to excessive width, then don't bother
				{
					#ifdef DEBUG
					y += IntToString(GetNextSentenceWidth(&source[x]) + temp + (spaceWidth * 2), &destination[y]);
					#endif
					// we are probably overestimating slightly here. but that's a lot better than underestimating and breaking a sentence in half.
					if (GetNextSentenceWidth(&source[x]) + temp + (spaceWidth * 2) > maxWidth)
					{
						destination[y++] = TEXT_A;
						wePressedA = true;
					}
				}
			}
			
			destination[y++] = TEXT_N;
			
			weLinebroke = true;
		}
		else
		{
			currentWidth += temp;
			destination[y++] = source[x++];			
		}
	}
	
	destination[y] = TEXT_X;
}

void AutoNewline(char* source, char* destination)
{
	if (IsTextStartCode(source[0]))
	{
		HandleAutoNewline(source, destination);
	}
	else
	{
		StringCopy(source, destination);
	}
}