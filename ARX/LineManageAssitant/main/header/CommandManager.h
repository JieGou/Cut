#pragma once

#include <map>
#include <string>

#include <accmd.h>

using namespace std;

class CommandManager
{
public:

	static CommandManager* gCmdManager;

	static CommandManager* instance();
	static void Release();

	void RegisterCommand() const;
	void UnRegisterCommand() const;

	static void ShowConfigDialog();

	static const WCHAR* CMD_GROUP;

	static const WCHAR* CMD_LINE_CONFIG; 
	static const WCHAR* CMD_LINE_INPUT;
	static const WCHAR* CMD_LIEN_CUT;

private:

	CommandManager(void);
	~CommandManager(void);

	//list to contains the commands supported
	map<wstring,AcRxFunctionPtr> mSupportCommands;
};
