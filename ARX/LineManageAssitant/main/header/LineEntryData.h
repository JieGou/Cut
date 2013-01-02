// ------------------------------------------------
//                  LineManagementAssistant
// Copyright 2012-2013, Chengyong Yang & Changhai Gu. 
//               All rights reserved.
// ------------------------------------------------
//	LineEntryData.h
//	written by Changhai Gu
// ------------------------------------------------
// $File:\\LineManageAssitant\main\header\LineEntryData.h $
// $Author: Changhai Gu $
// $DateTime: 2013/1/2 01:35:46 $
// $Revision: #1 $
// ------------------------------------------------
#pragma once

#include "stdafx.h"

#include <string>
#include <vector>

#include <dbsymtb.h>
#include <dbapserv.h>
#include <adslib.h>
#include <adui.h>
#include <acui.h>

using namespace std;

namespace com
{

namespace guch
{

namespace assistant
{

namespace data
{

/**
 * 管线坐标实体
 */
struct PointEntry
{
	//点号
	UINT m_PointNO;
	ads_point m_Point; 

	wstring m_LevelKind;
	wstring m_Direction;

	PointEntry();
	PointEntry( const UINT& pointNO, const ads_point& point, const wstring& levelKind, const wstring& direction);
	PointEntry( const PointEntry& );
	PointEntry( const wstring& data );

	wstring toString() const;
};

typedef PointEntry *pPointEntry;

typedef vector<pPointEntry> PointList;
typedef PointList::iterator PointIter;
typedef PointList::const_iterator ContstPointIter;

/**
 * 管线实体
 */
class LineEntry
{
public:
	LineEntry();
	LineEntry( const wstring& rLineNO,
				const wstring& rLineName,
				const wstring& rLineKind);

	LineEntry( const wstring& data);

	~LineEntry();

	void SetName( const wstring& rNewName ) { m_LineName = rNewName; }

	int InsertPoint( const PointEntry& newPoint );
	void UpdatePoint( const PointEntry& updatePoint );
	void DeletePoint( const UINT& PointNO );

	PointIter FindPoint( const UINT& PointNO ) const;
	ContstPointIter FindConstPoint( const UINT& PointNO ) const;

	wstring toString();

public:

	UINT m_LineID;

	wstring m_LineNO;
	wstring m_LineName;
	wstring m_LineKind;

	UINT m_CurrentPointNO;

	PointList* m_PointList;
};

typedef vector<LineEntry*> LineList;
typedef LineList::iterator LineIterator;
typedef LineList::const_iterator ConstLineIterator;

/**
 * 管线实体文件
 */
class LineEntryFile
{
public:
	LineEntryFile(const wstring& fileName);
	~LineEntryFile();

	void InsertLine( LineEntry* lineEntry);
	BOOL DeleteLine( const UINT& lineID );

	LineIterator FindLinePos( const UINT& lineID ) const;
	LineIterator FindLinePosByNO( const wstring& lineNO ) const;
	LineIterator FindLinePosByName( const wstring& lineName ) const;

	LineEntry* FindLine( const UINT& PointNO ) const;
	LineEntry* FindLineByNO( const wstring& lineName  ) const;
	LineEntry* FindLineByName( const wstring& lineName  ) const;

	void Init();
	void Persistent() const;

	LineList* GetList() const {return m_LineList;}

private:

	wstring m_FileName;
	LineList* m_LineList;
};

} // end of data

} // end of assistant

} // end of guch

} // end of com
