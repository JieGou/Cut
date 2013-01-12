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
//----- CCmdMonitorReactor.h : Declaration of the CCmdMonitorReactor
//-----------------------------------------------------------------------------
#pragma once

#ifdef CH09_MODULE
#define DLLIMPEXP __declspec(dllexport)
#else
//----- Note: we don't use __declspec(dllimport) here, because of the
//----- "local vtable" problem with msvc. If you use __declspec(dllimport),
//----- then, when a client dll does a new on the class, the object's
//----- vtable pointer points to a vtable allocated in that client
//----- dll. If the client dll then passes the object to another dll,
//----- and the client dll is then unloaded, the vtable becomes invalid
//----- and any virtual calls on the object will access invalid memory.
//-----
//----- By not using __declspec(dllimport), we guarantee that the
//----- vtable is allocated in the server dll during the ctor and the
//----- client dll does not overwrite the vtable pointer after calling
//----- the ctor. And, since we expect the server dll to remain in
//----- memory indefinitely, there is no problem with vtables unexpectedly
//----- going away.
#define DLLIMPEXP
#endif

//-----------------------------------------------------------------------------
#include "aced.h"

//-----------------------------------------------------------------------------
//----- Note: Uncomment the DLLIMPEXP symbol below if you wish exporting
//----- your class to other ARX/DBX modules
    //-------------------------------------------------------------------------------------------
	// 
	//  功能： 编辑器反应器，监控命令的执行
	//                 
	//
	//  作者： 
	//
	// 日期：200709
	//
	//  历史：无
	//
	//----------------------------------------------------------------------------------------------
class /*DLLIMPEXP*/ CCmdMonitorReactor : public AcEditorReactor {

public:
	ACRX_DECLARE_MEMBERS(CCmdMonitorReactor) ;

protected:
	//----- Auto initialization and release flag.
	bool mbAutoInitAndRelease ;

public:
	CCmdMonitorReactor (const bool autoInitAndRelease =true) ;
	virtual ~CCmdMonitorReactor () ;

	virtual void Attach () ;
	virtual void Detach () ;
	virtual AcEditor *Subject () const ;
	virtual bool IsAttached () const ;

public:
	// -----------------------------------------------------------------------------
	virtual void commandWillStart(const ACHAR * cmdStr);
public:
	// -----------------------------------------------------------------------------
	virtual void commandEnded(const ACHAR * cmdStr);
} ;

#ifdef CH09_MODULE
ACDB_REGISTER_OBJECT_ENTRY_AUTO(CCmdMonitorReactor)
#endif
