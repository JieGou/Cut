#include "stdafx.h"
#include "CommandManager.h"

#include "LineManageAssitant.h"

#include "AsdkAcUiDialogSample.h"
#include "AcExtensionModule.h"

#include "LMALineConfigManagerDialog.h"
#include "LineConfigDialog.h"

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
	mSupportCommands[CMD_LINE_INPUT] = ShowConfigDialog;
	mSupportCommands[CMD_LIEN_CUT] = ShowConfigDialog;
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
}
