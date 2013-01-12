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
//----- CCmdMonitorReactor.cpp : Implementation of CCmdMonitorReactor
//-----------------------------------------------------------------------------
#include "StdAfx.h"
#include "CCmdMonitorReactor.h"
# include <tchar.h>
//-----------------------------------------------------------------------------
ACRX_CONS_DEFINE_MEMBERS(CCmdMonitorReactor, AcEditorReactor, 1)

//-----------------------------------------------------------------------------
CCmdMonitorReactor::CCmdMonitorReactor (const bool autoInitAndRelease) : AcEditorReactor(), mbAutoInitAndRelease(autoInitAndRelease) {
	if ( autoInitAndRelease ) {
		if ( acedEditor )
			acedEditor->addReactor (this) ;
		else
			mbAutoInitAndRelease =false ;
	}
}

//-----------------------------------------------------------------------------
CCmdMonitorReactor::~CCmdMonitorReactor () {
	Detach () ;
}

//-----------------------------------------------------------------------------
void CCmdMonitorReactor::Attach () {
	Detach () ;
	if ( !mbAutoInitAndRelease ) {
		if ( acedEditor ) {
			acedEditor->addReactor (this) ;
			mbAutoInitAndRelease =true ;
		}
	}
}

void CCmdMonitorReactor::Detach () {
	if ( mbAutoInitAndRelease ) {
		if ( acedEditor ) {
			acedEditor->removeReactor (this) ;
			mbAutoInitAndRelease =false ;
		}
	}
}

AcEditor *CCmdMonitorReactor::Subject () const {
	return (acedEditor) ;
}

bool CCmdMonitorReactor::IsAttached () const {
	return (mbAutoInitAndRelease) ;
}

// -----------------------------------------------------------------------------
void CCmdMonitorReactor::commandWillStart(const ACHAR * cmdStr)
{
	if(wcscmp(cmdStr, _T("MOVE"))==0 )
	{
		// 如果是MOVE，提示用户
		acutPrintf(_T("\n开始执行MOVE命令:\n"));
	}

}

// -----------------------------------------------------------------------------
void CCmdMonitorReactor::commandEnded(const ACHAR * cmdStr)
{
	if(wcscmp(cmdStr, _T("MOVE"))==0 )
	{
		// 如果是MOVE，提示用户
		acutPrintf(_T("\n执行MOVE命令完毕.。\n"));
	}
}
