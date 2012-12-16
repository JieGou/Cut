
#include "CutCommandMgr.h"

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

typedef map<string,string>::iterator CommandIterator;

CutCommandMgr* CutCommandMgr::mInstance = NULL;

CutCommandMgr::CutCommandMgr(void)
{
	//draw line
	//mSupportCommands.insert(CMD_CUT_GROUP,CMD_CUT_DRAWLINE);
	mSupportCommands = map<string,string>();
	mSupportCommands.insert(pair<string,string>("",""));
}

CutCommandMgr::~CutCommandMgr(void)
{
}

CutCommandMgr* CutCommandMgr::instance()
{
	if( mInstance == NULL)
	{
		mInstance = new CutCommandMgr();
	}

	return mInstance;
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

void drawLine()
{
	acutPrintf(L"Draw the circle.\n");

	double radius = 4000;
	acedCommand(RTSTR,L"CIRCLE",RTSTR,L"280,150",RTREAL,radius,0);

	acutPrintf(L"DONE! Circle draw.\n");
}

static void drawLines(ads_callback_packet* cpkt)
{
	acutPrintf(L"Get values for the fill points.\n");

	ACHAR pointsBuf[6][MAX_TILE_STR];

	ads_get_tile(cpkt->dialog,L"P1X",pointsBuf[0],MAX_TILE_STR);
	ads_get_tile(cpkt->dialog,L"P1Y",pointsBuf[1],MAX_TILE_STR);
	ads_get_tile(cpkt->dialog,L"P1Z",pointsBuf[2],MAX_TILE_STR);
	
	ads_get_tile(cpkt->dialog,L"P2X",pointsBuf[3],MAX_TILE_STR);
	ads_get_tile(cpkt->dialog,L"P2Y",pointsBuf[4],MAX_TILE_STR);
	ads_get_tile(cpkt->dialog,L"P2Z",pointsBuf[5],MAX_TILE_STR);

	acutPrintf(L"User fill points:\n");
	acutPrintf(L"Point 1 X[%s] Y[%s] Z[%s]:\n",pointsBuf[0],pointsBuf[1],pointsBuf[2]);
	acutPrintf(L"Point 2 X[%s] Y[%s] Z[%s]:\n",pointsBuf[3],pointsBuf[4],pointsBuf[5]);

	acutPrintf(L"DONE! Get values.\n");
}

static void doneFillPoints(ads_callback_packet* cpkt)
{
	ads_done_dialog(cpkt->dialog,1);
}

void fillPoint()
{
	acutPrintf(L"Show dialog to fill points.\n");

	ads_hdlg hDlg;
	int dcl_id, dlg_status;

	//load the dialog
	ads_load_dialog(L"fillpoints_dlg.dcl",&dcl_id);
	ads_new_dialog(L"fillpoints",dcl_id,(CLIENTFUNC)0,&hDlg);
	ads_action_tile(hDlg,L"draw",(CLIENTFUNC)drawLines);
	ads_action_tile(hDlg,L"done",(CLIENTFUNC)doneFillPoints);
	ads_start_dialog(hDlg,&dlg_status);
	ads_unload_dialog(dcl_id);

	acutPrintf(L"DONE! Fill points.\n");
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
	moveToBottom(pSelectionRegion);
	
	//将截面加入到模型空间
	PostToModelSpace(pSelectionRegion);
}

void CutCommandMgr::registerCommands()
{
	acutPrintf(L"Begin register all commands supported, amount is [%d].\n",this->mSupportCommands.size());

	for(CommandIterator iter = this->mSupportCommands.begin(); iter != this->mSupportCommands.end(); iter++)
	{
		acutPrintf(L"\n");

		//画直线
		acedRegCmds->addCommand(CMD_CUT_GROUP,CMD_CUT_DRAWLINE,CMD_CUT_DRAWLINE,ACRX_CMD_TRANSPARENT,drawLine);
	}

	//填出对话框
	acedRegCmds->addCommand(CMD_CUT_GROUP,CMD_CUT_FILLPOINT,CMD_CUT_FILLPOINT,ACRX_CMD_TRANSPARENT,fillPoint);

	//画圆柱体
	acedRegCmds->addCommand(CMD_CUT_GROUP,CMD_LMHS_DRAW_CYLINDER,CMD_LMHS_DRAW_CYLINDER,ACRX_CMD_TRANSPARENT,drawCylinder);

	acutPrintf(L"DONE! Register all commands supported.\n",this->mSupportCommands.size());
}
