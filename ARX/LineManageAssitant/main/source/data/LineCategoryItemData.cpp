#include "stdafx.h"
#include "LineCategoryItemData.h"

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

LineCategoryItemData::LineCategoryItemData(void)
{
}

LineCategoryItemData::LineCategoryItemData(
							const wstring& rID,
							const wstring& rName,
							const wstring& rKind,
							const wstring& rCategory,
							const wstring& rShape,
							const wstring& rSize,
							const wstring& rEffectSize,
							const wstring& rUnit,
							const wstring& rComment)
:mID(rID),
mName(rName),
mKind(rKind),
mCategory(rCategory),
mShape(rShape),
mSize(rSize),
mEffectSize(rEffectSize),
mUnit(rUnit),
mComment(rComment)
{}

wstring LineCategoryItemData::toString() const
{
	return L"";
}

LineCategoryItemData::~LineCategoryItemData(void){}

} // end of data

} // end of assistant

} // end of guch

} // end of com