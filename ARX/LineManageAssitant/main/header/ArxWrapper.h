// ------------------------------------------------
//                  LineManagementAssistant
// Copyright 2012-2013, Chengyong Yang & Changhai Gu. 
//               All rights reserved.
// ------------------------------------------------
//	ArxWrapper.h
//	written by Changhai Gu
// ------------------------------------------------
// $File:\\LineManageAssitant\main\source\ARX\ArxWrapper.h $
// $Author: Changhai Gu $
// $DateTime: 2013/1/2 01:35:46 $
// $Revision: #1 $
// ------------------------------------------------

#include "afxcmn.h"

#include <string.h>
#include <aced.h>
#include <dbents.h>
#include <dbsymtb.h>
#include <dbgroup.h>
#include <dbapserv.h>
#include "tchar.h"

#include <string>
#include <vector>

#include <LineEntryData.h>

using namespace com::guch::assistant::data;
using namespace std;

typedef vector<AcGePoint3d*> Point3dVector;
typedef Point3dVector::const_iterator Point3dIter;

class ArxWrapper
{
public:
	static void createNewLayer(const wstring& layerName);

	static AcDbObjectId createLine( const AcGePoint3d& start,
							const AcGePoint3d& end,
							const wstring& layerName );

	static void createLine( const Point3dVector& points3d,
							const wstring& layerName );

	static void createLine ( const PointList& points,
							const wstring& layerName);

	//Add entry to model
	static AcDbObjectId PostToModelSpace(AcDbEntity* pEnt,const wstring& layerName );

	//Remove entry from model
	static Acad::ErrorStatus RemoveFromModelSpace(AcDbEntity* pEnt,const wstring& layerName );

	//Add object to name dictionary
	static AcDbObjectId PostToNameObjectsDict(AcDbObject* pObj,const wstring& key, bool toDelete = false );

	//Read object from name dictionary
	static void PullFromNameObjectsDict();

	//move offset
	static AcDbEntity* MoveToBottom(AcDbEntity* pEntry);

	//Create LMA Line
	static void createLMALine( const LineEntry& entry );

	//Polygon cylinder
	static void DrawPolyCylinder( const LineEntry& lineEntry, const double& radius);

	//cylinder
	static AcDb3dSolid* DrawCylinder( const UINT& lineID,
										const UINT& sequenceID,
										const AcGePoint3d& start,
										const AcGePoint3d& end,
										const wstring& layerName,
										const double& radius );

	static void eraseLMALine(const LineEntry& lineEntry, bool old = false);
};
