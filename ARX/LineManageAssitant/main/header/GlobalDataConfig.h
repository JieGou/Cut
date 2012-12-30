#include "stdafx.h"
#include <string>

using namespace std;

namespace com
{

namespace guch
{

namespace assistent
{

namespace data
{

class GlobalData
{
public:
	
	/**
	* 管线类型
	**/
	static const wstring KIND_LINE;
	static const wstring KIND_SEPERATOR;

	/**
	* 管道种类
	**/
	static const wstring LINE_CATEGORY_SHANGSHUI;
	static const wstring LINE_CATEGORY_XIASHUI;
	static const wstring LINE_CATEGORY_NUANQI;
	static const wstring LINE_CATEGORY_DIANLAN;
	static const wstring LINE_CATEGORY_YUSUI;
	static const wstring LINE_CATEGORY_TONGXIN;

	/**
	* 管道形状
	**/
	static const wstring LINE_SHAPE_CIRCLE;
	static const wstring LINE_SHAPE_SQUARE;

	/**
	* 管道单位
	**/
	static const wstring LINE_UNIT_MM;
	static const wstring LINE_UNIT_CM;
	static const wstring LINE_UNIT_M;

	/**
	* 字符长度
	**/
	static const UINT ITEM_TEXT_MAX_LENGTH;
};


} // end of data

} // end of assistant

} // end of guch

} // end of com