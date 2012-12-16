#pragma once

#define CMD_CUT_GROUP L"cutGroup"
#define CMD_CUT_DRAWLINE L"drawline"

#define CMD_CUT_FILLPOINT L"fillpoint"

#define CMD_LMHS_DRAW_CYLINDER L"yuanzhu"

#include <map>
#include <string>

using namespace std;

class CutCommandMgr
{
private:
	static CutCommandMgr* mInstance;

	//list to contains the commands supported
	map<string,string> mSupportCommands;
	
	CutCommandMgr(void);
	~CutCommandMgr(void);

public:
	
	static CutCommandMgr* instance();
	void registerCommands();
};

