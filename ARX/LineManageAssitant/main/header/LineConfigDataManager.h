#pragma once

#include <vector>
#include <LineCategoryItemData.h>

namespace com
{

namespace guch
{

namespace assistent
{

namespace config
{

typedef vector<LineCategoryItemData*>* LineCategoryVecotr;

class LineConfigDataManager
{
public:
	LineConfigDataManager(void);
	~LineConfigDataManager(void);

	static LineConfigDataManager* Instance();
	static const string PERSISTENT_FILE;

	/**
	 * Save the configuration data to file
	 **/
	bool persistent();

	/**
	 * Save the configuration data to file
	 **/
	const LineCategoryVecotr GetData() const;

protected:

	bool initialize();

private:

	static LineConfigDataManager* instance;

	LineCategoryVecotr mLineConfigData;
};

} // end of data

} // end of assistant

} // end of guch

} // end of com
