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

extern u8 MaxConversationWidth;
extern u8 MaxBattleWidth;


extern u8 DynamicTextboxSize;
extern u8 MinimumSentenceWidth;
extern u8 BreakEarlyToPreserveSentence;
extern u8 SingleLineBreakWidth;
extern u8 DontPauseBeforeA;

extern u8 BeatPause;
extern u8 PunctuationPause;

extern u8 DebugOutput;
extern char* RAMLocation;

extern u8 IsValidASCIITable[];

void PostprocessString(const char* source, char* destination);

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

bool IsValidASCII(const char source)
{
	return IsValidASCIITable[(u8)source];
	//return (source >= 0x20 && source <= 0x7F);
}

bool IsOpenLocation(const char source)
{
	return (source >= 0x08 && source <= 0x0F);
}

bool IsTextStartCode(const char source)
{
	return (source == Text_Conversation || source == Text_Battle);
}

bool IsPrintable(const char* source)
{
	return (IsValidASCII(source[0]) && source[-1] != TEXT_LOADFACE);
}

bool IsLoadFaceParam(const char* source)
{
	if (source[-2] == TEXT_LOADFACE || source[-1] == TEXT_LOADFACE)
		return true;
	else
		return false;
}

bool IsTextCodeParam(const char* source)
{
	if (IsLoadFaceParam(source) || source[-1] == 0x80)
		return true;
	else
		return false;
}

bool IsTextboxEndCode(const char* source)
{
	if (IsTextCodeParam(source))
		return false;
	else if (IsOpenLocation(*source) || *source == TEXT_CLEARFACE || *source == TEXT_CLOSESPEECHFAST || *source == TEXT_CLOSESPEECHSLOW || *source == TEXT_X)
		return true;
	else
		return false;
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

bool IsTextEndSoon(const char* source)
{
	for (int x = 1; source[x] != TEXT_X; x++)
	{
		if (IsPrintable(&source[x]))
			return false;
	}
	
	return true;
}

bool IsTextboxEndSoon(const char* source)
{
	for (int x = 1; source[x] != TEXT_X; x++)
	{
		if (IsPrintable(&source[x]))
			return false;
		else if (IsTextboxEndCode(&source[x]))
			return true;
	}

	return true;
}

bool IsSentenceEnd(const char* source)
{
	if (IsLinebreakPunctuation(LastPrintable(source)) && !IsLinebreakPunctuation(source[0]))
		return true;
	else
		return false;
}

int GetNextTextboxWidth(const char* source, int maxWidth)
{
	int totalWidth = 0;
	int newMaxWidth = 0;

	int sentenceWidth = 0;
	int totalWidthAtLastSentence = 0;
	unsigned temp = 0;

	const char aSpace = ' ';
	unsigned spaceWidth = 0;
	GetCharTextWidthAscii(&aSpace, &spaceWidth);

	for (int x = 0; !IsTextboxEndCode(&source[x]); x++)
	{
		if (IsPrintable(&source[x]))
		{
			if (totalWidth > (maxWidth * 2) - 25)
			{
				totalWidth = totalWidthAtLastSentence;
				break; // too big, use the total width at last sentence
			}
			else
			{
				if (IsSentenceEnd(&source[x]) && sentenceWidth > MinimumSentenceWidth)
				{
					sentenceWidth = 0;
					totalWidthAtLastSentence = totalWidth;
				}

				GetCharTextWidthAscii(&source[x], &temp);
				sentenceWidth += temp;
				totalWidth += temp;
			}
		}
	}

	if (totalWidth > maxWidth || totalWidth > SingleLineBreakWidth)
	{
		int currentWidth = 0;
		newMaxWidth = (totalWidth / 2);

		if (totalWidth % 2 != 0)
			newMaxWidth += 1;
		//newMaxWidth = (totalWidth / 2) + 25; // this "+20" stuff needs to be replaced by something more sophisticated. like allowing the last word on line 1 to exceed maxWidth.

		// now we have to increase the width so that the first line ends up longer, and so that everything fits
		// otherwise we could end up without enough space, thanks to having to go back and break on a space ending line 1 early.
		for (int x = 0; !IsTextboxEndCode(&source[x]); x++)
		{
			if (IsPrintable(&source[x]))
			{
				GetCharTextWidthAscii(&source[x], &temp);
				currentWidth += temp;

				if (currentWidth > newMaxWidth)
				{
					if (source[x] == ' ')
						break;
					else
						newMaxWidth += temp;
				}
			}
		}

		//newMaxWidth += 3;

		if (newMaxWidth > maxWidth)
			newMaxWidth = maxWidth;
	}
	else
		newMaxWidth = maxWidth;

	return newMaxWidth;
}

// this function handles adding [A][N] and spaces where necessary in response to control codes.
const char* PreprocessString(const char* source, char *destination)
{
	int x = 0;
	int y = 0;	
	bool midLine = false;
	
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
				
				// so we need to handle "Some Text.... Some more text." by inserting a space after the first ... (because it almost certainly was "Some Text.\n... Some more text" pre textprocess)
				// at the same time we need to handle "Some Text...?" by not inserting a space until it's over.
				// so we explicitly check for four periods in a row
				if (periodCount == 4)
				{
					destination[y++] = source[x++]; // copy the period
					destination[y++] = ' ';
					
					periodCount--; // because we'll always want to trigger the beat handling.
				}
				
				while (periodCount > 0 && periodCount % 3 == 0)
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
					destination[y++] = TEXT_TOGGLEMOUTHMOVE;
					destination[y++] = PunctuationPause;
					destination[y++] = TEXT_TOGGLEMOUTHMOVE;
				}
			}
			else if (source[x] == '-') // handle '--' (typically has [ToggleMouthMove] wrapped around it)
			{
				destination[y++] = TEXT_TOGGLEMOUTHMOVE;
				while (source[x] == '-')
					destination[y++] = source[x++];
				destination[y++] = TEXT_TOGGLEMOUTHMOVE;
			}
			// putting "Yo what's up.\nNot much." into textprocess will give you "Yo what's up.Not much." in game.
			// therefore it is necessary to automatically insert spaces after punctuation, if we are not linebreaking.
			else if (IsSentenceEnd(&source[x]) && source[x] != ' ' && !IsTextboxEndCode(&source[x]))
			{
				destination[y++] = ' ';
				destination[y++] = source[x++];
			}
			else // something that doesn't need special handling
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
			else if (midLine && IsTextboxEndCode(&source[x])) // handle codes that (at least typically) end the textbox
			{
				destination[y++] = TEXT_A;
				if (DynamicTextboxSize)
					destination[y++] = TEXT_CLOSESPEECHSLOW;
				else
					destination[y++] = TEXT_N; // needed in some edge/unusual cases
				destination[y++] = source[x++];
				
				midLine = false;
			}
			else if (source[x] == 0x80 && source[x + 1] == 0x20) // handle [Tact]
			{
				for (int z = 0; z < 9 && IsValidASCII(gChapterData.playerName[z]); z++)
				{
					destination[y++] = gChapterData.playerName[z];
				}
				source += 2;
			}
			else if (source[x] == 0x80) // handle the two byte codes
			{
				if (source[x + 1] == 0x04 && midLine) // handle [LoadOverworldFaces]
				{
					destination[y++] = TEXT_A;		
					if (DynamicTextboxSize)
						destination[y++] = TEXT_CLOSESPEECHSLOW;
					else
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

void HandleAutoNewline(const char* input, char* fDestination)
{
	const u8 textType = input[1];
	input += 2; // skip past the textType short
	int hardMaxWidth = 0;
	
	if (textType == Text_Conversation)
		hardMaxWidth = MaxConversationWidth;
	else //if (textType == Text_Battle)
		hardMaxWidth = MaxBattleWidth;
	
	const char* source = PreprocessString(input, fDestination);
	char* destination = RAMLocation;
	
	int x = 0;
	int y = 0;
	unsigned currentWidth = 0;
	unsigned currentSentenceWidth = 0;
	unsigned temp = 0;
	bool lineTwo = false;
	bool insertLineBreak = false;
	bool insertAPress = false;
	bool wePressedA = false;
	bool weLinebroke = false;

	int xLastSentence = 0;
	int yLastSentence = 0;
	unsigned lastSentenceWidth = 0;
	
	int lastSpaceSource = 0;
	int lastSpaceDestination = 0;

	int maxWidth = hardMaxWidth;
	bool midTextbox = false;
	
	const char aSpace = ' ';
	unsigned spaceWidth = 0;
	GetCharTextWidthAscii(&aSpace, &spaceWidth);
	
	// handle non printable strings.
	// this double checks that we're not text skipping
	// and makes sure the rest of the code knows the current state of the text
	while (source[x] != TEXT_X)
	{
		while (!IsPrintable(&source[x]))
		{
			if ((source[x] == TEXT_A || source[x] == TEXT_N || source[x] == TEXT_CLEAR) && !IsTextCodeParam(&source[x]))
			{
				// multiple [A] or [N] in the same non printable string. skip it.
				if ((wePressedA && source[x] == TEXT_A) || (weLinebroke && source[x] == TEXT_N)) 
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
					else if(source[x] == TEXT_N || source[x] == TEXT_CLOSESPEECHSLOW)
					{
						weLinebroke = true;
						
						if (lineTwo && !wePressedA) // two [N]s without an [A] is a text skip and text skips are bad. so put the [A] in.
						{
							destination[y++] = TEXT_A;
						}
					}
					else if (source[x] == TEXT_CLEAR)
					{
						destination[y++] = TEXT_A;
						destination[y++] = TEXT_CLEAR;
						destination[y++] = TEXT_N;
						weLinebroke = true;
						wePressedA = true;
					}
					
					destination[y++] = source[x++];
				}
			}
			else if (source[x] == TEXT_X) // if we hit the 'end of text' code, we're just outta here
			{
				destination[y++] = TEXT_X;
				PostprocessString(destination, fDestination);
				return;
			}
			else
			{
				destination[y++] = source[x++];
			}
		}

		// now we've arrived at a printable character
		GetCharTextWidthAscii(&source[x], &temp);

		// if-else chain 1
		// - if we broke and need to cleanup, do cleanup
		// - else we decide if we need to break
		if (IsTextEndSoon(&source[x]))
		{
			insertLineBreak = false; // don't bother doing any more special handling. we're done here.
		}
		else if (weLinebroke || wePressedA)
		{
			if (wePressedA)
			{
				lineTwo = false;
				midTextbox = false;
				currentSentenceWidth = 0;
				xLastSentence = 0;
				yLastSentence = 0;
			}
			else
			{
				lineTwo = true;
			}
			
			// skip spaces at the start of new lines
			while (source[x] == ' ')
				x++;
			
			currentWidth = 0;
			lastSentenceWidth = 0;
			insertLineBreak = false;
			insertAPress = false;
			lastSpaceSource = 0;
			lastSpaceDestination = 0;
			
			weLinebroke = false;
			wePressedA = false;
		}
		
		if (source[x] == ' ')
		{
			lastSpaceSource = x;
			lastSpaceDestination = y;
		}

		if (IsSentenceEnd(&source[x]))
		{
			// so this is a potential line break spot (sentence end)
			if (currentSentenceWidth > MinimumSentenceWidth)
			{
				xLastSentence = x;
				yLastSentence = y;
				lastSentenceWidth = currentSentenceWidth;
				currentSentenceWidth = 0;
			}
		}
		
		if (currentWidth > maxWidth)
		{
			// this ensures we don't try to cram part of the next sentence into our textbox (even if there's room)
			if (lineTwo && DynamicTextboxSize && xLastSentence)
			{
				x = xLastSentence;
				y = yLastSentence;
				lastSpaceSource = 0;
				lastSpaceDestination = 0;
				insertAPress = true;
			}
			// this code is supposed to break early line one to encourage sentences to be on their own lines
			// But it really needs to be replaced by something more sophisticated.
			else if (!lineTwo && DynamicTextboxSize && lastSentenceWidth > maxWidth - BreakEarlyToPreserveSentence)
			{
				if (maxWidth + BreakEarlyToPreserveSentence < hardMaxWidth)
					maxWidth += BreakEarlyToPreserveSentence;

				x = xLastSentence;
				y = yLastSentence;
				lastSpaceSource = 0;
				lastSpaceDestination = 0;
			}

			insertLineBreak = true;
		}
		
		if (DynamicTextboxSize && textType == Text_Conversation && !midTextbox) // new textbox
		{
			midTextbox = true;

			maxWidth = GetNextTextboxWidth(&source[x], hardMaxWidth);

			if (DebugOutput)
			{
				y += IntToString(maxWidth, &destination[y]);
				destination[y++] = ' ';
			}

			if (maxWidth > hardMaxWidth)
				maxWidth = hardMaxWidth;
		}
		
		if (insertLineBreak) 
		{
			if (lineTwo)
				insertAPress = true;

			if (lastSpaceSource && lastSpaceDestination) 
			{
				x = lastSpaceSource;
				y = lastSpaceDestination;
				x++; // don't copy the space
			}

			if (insertAPress)
			{
				destination[y++] = TEXT_A;
				wePressedA = true;
			}

			if (DynamicTextboxSize && textType == Text_Conversation && insertAPress)
				destination[y++] = TEXT_CLOSESPEECHSLOW;
			else
				destination[y++] = TEXT_N;
			
			weLinebroke = true;
		}
		else
		{
			currentWidth += temp;
			currentSentenceWidth += temp;
			destination[y++] = source[x++];			
		}
	}
	
	destination[y] = TEXT_X;
	PostprocessString(destination, fDestination);
}

void PostprocessString(const char* source, char* destination)
{
	int x = 0;
	int y = 0;

	while (source[x] != TEXT_X)
	{
		if (IsTextboxEndSoon(&source[x]) && !IsTextCodeParam(&source[x]) && (source[x] == PunctuationPause || source[x] == BeatPause))
		{
			x++;
		}
		else
		{
			destination[y++] = source[x++];
		}
	}

	destination[y] = TEXT_X;
}

void AutoNewline(char* source, char* destination)
{
	if (source[0] == 0x80 && IsTextStartCode(source[1]))
	{
		HandleAutoNewline(source, destination);
	}
	else
	{
		StringCopy(source, destination);
	}
}
