#include "stdafx.h"
#include "CommandManager.h"

#include "LineManageAssitant.h"

#include "AsdkAcUiDialogSample.h"
#include "AcExtensionModule.h"

#include "LMALineConfigManagerDialog.h"
#include "LineConfigDialog.h"
#include "AddLineDialog.h"

#include "acedads.h"
#include "accmd.h"
#include <adscodes.h>

#include <adsdlg.h>

#include <dbapserv.h>

#include <dbregion.h>

#include <gepnt3d.h>

//symbol table
#include <dbsymtb.h>

//3D Object
#include <dbsol3d.h>

using namespace com::guch::assistent::config;

typedef map<wstring,AcRxFunctionPtr>::const_iterator CommandIterator;

CommandManager* CommandManager::gCmdManager = NULL;

const WCHAR* CommandManager::CMD_GROUP = L"LMA_CMD_GROUP";
const WCHAR* CommandManager::CMD_LINE_CONFIG = L"LMA_CONFIG";
const WCHAR* CommandManager::CMD_LINE_INPUT = L"LMA_INPUT";
const WCHAR* CommandManager::CMD_LIEN_CUT = L"LMA_CUT";

CommandManager* CommandManager::instance()
{
	if( gCmdManager == NULL )
	{
		gCmdManager = new CommandManager();
	}

	return gCmdManager;
}

void CommandManager::Release()
{
	if( gCmdManager )
	{
		delete gCmdManager;
		gCmdManager = NULL;
	}
}

CommandManager::CommandManager(void)
{
	mSupportCommands[CMD_LINE_CONFIG] = ShowConfigDialog;
	mSupportCommands[CMD_LINE_INPUT] = ShowAddLineDialog;
	mSupportCommands[CMD_LIEN_CUT] = GenerateCut;
}

CommandManager::~CommandManager(void)
{
}

void CommandManager::RegisterCommand() const
{
	for( CommandIterator iter = this->mSupportCommands.begin();
		iter != this->mSupportCommands.end();
		iter++)
	{
		CAcModuleResourceOverride resOverride;

		CString globalCmd;
		globalCmd.Format(L"G_%s",iter->first.c_str());

		acedRegCmds->addCommand(CMD_GROUP,globalCmd,
			iter->first.c_str(),
			ACRX_CMD_MODAL,
			iter->second);
	}
}

void CommandManager::UnRegisterCommand() const
{
	acedRegCmds->removeGroup(CMD_GROUP);

	CommandManager::Release();
}

void CommandManager::ShowConfigDialog()
{
	// Modal
	LineConfigDialog dlg(CWnd::FromHandle(adsw_acadMainWnd()));
	INT_PTR nReturnValue = dlg.DoModal();
}

void CommandManager::ShowAddLineDialog()
{
	AddLineDialog dlg(CWnd::FromHandle(adsw_acadMainWnd()));
	INT_PTR nReturnValue = dlg.DoModal();
}

AcDbObjectId PostToModelSpace(AcDbEntity* pEnt)
{
	AcDbBlockTable *pBlockTable;
	acdbHostApplicationServices()->workingDatabase()
		->getBlockTable(pBlockTable, AcDb::kForRead);

	AcDbBlockTableRecord *pBlockTableRecord;
	pBlockTable->getAt(ACDB_MODEL_SPACE, pBlockTableRecord,
	AcDb::kForWrite);

	AcDbObjectId entId;
	pBlockTableRecord->appendAcDbEntity(entId, pEnt);

	pBlockTable->close();
	pBlockTableRecord->close();
	pEnt->close();

	return entId;
}

void moveToBottom(AcDbEntity* pEntry)
{
	AcGeVector3d vec(-8,10,0);

	AcGeMatrix3d moveMatrix;
	moveMatrix.setToTranslation(vec);

	pEntry->transformBy(moveMatrix);
}

void drawCylinder()
{
	// 创建特定参数的圆柱体（实际上最后一个参数决定了实体是一个圆锥体还是圆柱） 
	AcDb3dSolid *pSolid = new AcDb3dSolid(); 
	pSolid->createFrustum(30, 10, 10, 10);

	// 将圆锥体添加到模型空间
	PostToModelSpace(pSolid);

	//创建切面
	AcGePlane plane;
    plane.set(AcGePoint3d(8,0,0),AcGeVector3d(1,0,0));

	//得到实体与切面相切的截面
	AcDbRegion *pSelectionRegion = NULL;
	pSolid->getSection(plane, pSelectionRegion);

	//将其移动到YZ平面
	//moveToBottom(pSelectionRegion);
	
	//将截面加入到模型空间
	PostToModelSpace(pSelectionRegion);
}

void CommandManager::GenerateCut()
{
	drawCylinder();
}
