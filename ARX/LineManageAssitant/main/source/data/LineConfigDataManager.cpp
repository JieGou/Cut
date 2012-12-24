#include "stdafx.h"

#include <LineConfigDataManager.h>

#include <GlobalDataConfig.h>

using namespace com::guch::assistent::data;

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

LineConfigDataManager* LineConfigDataManager::instance = NULL;
const string LineConfigDataManager::PERSISTENT_FILE = "";

LineConfigDataManager* LineConfigDataManager::Instance()
{
	if( instance == NULL )
	{
		instance = new LineConfigDataManager();
	}

	return instance;
}

LineConfigDataManager::LineConfigDataManager(void)
{
	mLineConfigData = new vector<LineCategoryItemData*>();

	//read data from file PERSISTENT_FILE
#ifdef _DEMO_DATA
	const int MAX_ITEM = 10;

	for( int i = 0; i < MAX_ITEM; i++)
	{
		CString ID;
		ID.Format(L"%d",i);

		LineCategoryItemData* item = new LineCategoryItemData(wstring(ID.GetBuffer()), 
						L"测试管道",
						GlobalData::KIND_LINE,
						GlobalData::LINE_CATEGORY_SHANGSHUI,
						GlobalData::LINE_SHAPE_CIRCLE,
						L"15",
						L"5",
						GlobalData::LINE_UNIT_CM,
						L"测试数据");

		mLineConfigData->push_back(item);
	}
#else
	
#endif
}

LineConfigDataManager::~LineConfigDataManager(void)
{

}

const LineCategoryVecotr LineConfigDataManager::GetData() const
{
	return mLineConfigData;
}

} // end of data

} // end of assistant

} // end of guch

} // end of com
