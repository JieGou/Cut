#pragma once

/**
 * This class contains the data for the line configuration item
 **/

#include <string>

using namespace std;

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

struct LineCategoryItemData
{
	wstring mID;
	wstring mName;
	wstring mKind;
	wstring mCategory;
	wstring mShape;
	wstring mSize;
	wstring mEffectSize;
	wstring mUnit;
	wstring mComment;

	LineCategoryItemData(void);
	LineCategoryItemData(
							const wstring& rID,
							const wstring& rName,
							const wstring& rKind,
							const wstring& rCategory,
							const wstring& rShape,
							const wstring& rSize,
							const wstring& rEffectSize,
							const wstring& rUnit,
							const wstring& rComment);

	~LineCategoryItemData(void);

private:

	wstring toString() const;
};

} // end of data

} // end of assistant

} // end of guch

} // end of com
