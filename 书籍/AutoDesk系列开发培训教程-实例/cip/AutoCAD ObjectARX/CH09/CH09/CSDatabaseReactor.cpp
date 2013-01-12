// (C) Copyright 2002-2005 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

//-----------------------------------------------------------------------------
//----- CSDatabaseReactor.cpp : Implementation of CSDatabaseReactor
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "CSDatabaseReactor.h"
#include "tchar.h"
//-----------------------------------------------------------------------------
ACRX_CONS_DEFINE_MEMBERS(CSDatabaseReactor, AcDbDatabaseReactor, 1)

//-----------------------------------------------------------------------------
CSDatabaseReactor::CSDatabaseReactor (AcDbDatabase *pDb) : AcDbDatabaseReactor(), mpDatabase(pDb) {
	if ( pDb )
		pDb->addReactor (this) ;
}

//-----------------------------------------------------------------------------
CSDatabaseReactor::~CSDatabaseReactor () {
	Detach () ;
}

//-----------------------------------------------------------------------------
void CSDatabaseReactor::Attach (AcDbDatabase *pDb) {
	Detach () ;
	if ( mpDatabase == NULL ) {
		if ( (mpDatabase =pDb) != NULL )
			pDb->addReactor (this) ;
	}
}

void CSDatabaseReactor::Detach () {
	if ( mpDatabase ) {
		mpDatabase->removeReactor (this) ;
		mpDatabase =NULL ;
	}
}

AcDbDatabase *CSDatabaseReactor::Subject () const {
	return (mpDatabase) ;
}

bool CSDatabaseReactor::IsAttached () const {
	return (mpDatabase != NULL) ;
}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::objectAppended(const AcDbDatabase * dwg, const AcDbObject * dbObj)
{

	//如果是圆，改变圆的颜色为红色
	if(dbObj->isKindOf (AcDbCircle::desc ()) )
	{
		AcDbCircle* pCir = AcDbCircle::cast(dbObj);
		//升级打开模式,修改对象

		pCir->upgradeOpen ();
		pCir->setColorIndex (1);
		acutPrintf(_T("\n创建了圆\n"));

	}
	else if(dbObj->isKindOf (AcDbLine::desc ()))
	{ 
		//如果是直线，改变颜色为黄色
		AcDbLine * pLine= AcDbLine::cast(dbObj);
		//升级打开模式,修改对象
		pLine->upgradeOpen ();
		pLine->setColorIndex (2);
		acutPrintf(_T("\n创建了直线\n"));

	}

}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::objectModified(const AcDbDatabase * dwg, const AcDbObject * dbObj)
{
	AcDbDatabaseReactor::objectModified (dwg, dbObj) ;
}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::objectErased(const AcDbDatabase * dwg, const AcDbObject * dbObj, Adesk::Boolean pErased)
{
	AcDbDatabaseReactor::objectErased (dwg, dbObj, pErased) ;
}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::objectOpenedForModify(const AcDbDatabase * dwg, const AcDbObject * dbObj)
{
	AcDbDatabaseReactor::objectOpenedForModify (dwg, dbObj) ;
}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::headerSysVarWillChange(const AcDbDatabase * dwg, const ACHAR * name)
{
	AcDbDatabaseReactor::headerSysVarWillChange (dwg, name) ;
}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::headerSysVarChanged(const AcDbDatabase * dwg, const ACHAR * name, Adesk::Boolean bSuccess)
{
	AcDbDatabaseReactor::headerSysVarChanged (dwg, name, bSuccess) ;
}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::objectReAppended(const AcDbDatabase * dwg, const AcDbObject * dbObj)
{
	AcDbDatabaseReactor::objectReAppended (dwg, dbObj) ;



}

// -----------------------------------------------------------------------------
void CSDatabaseReactor::objectUnAppended(const AcDbDatabase * dwg, const AcDbObject * dbObj)
{
	AcDbDatabaseReactor::objectUnAppended (dwg, dbObj) ;
}
