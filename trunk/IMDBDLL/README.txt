If you want to use this API in your *NON-COMMERCIAL* application, you MUST have implemented these methods:

- public void processResult(XmlDocument xmlDoc)
  In it you have to process the XML document. I provide a file for the format of a TV serie document and another
  with the format of a movie. Just don't forget that some elements may not be in the document, if there were no 
  information found or if the corresponding field is set to false.
  
- public void progressConf(int value)
  {
      progressBar1.Maximum = value;
  }
  This is used to set the maximum value of the progress bar. If you don't have a progress bar, leave it blank,
  but you must have the method defined.
  
- public void progressUpdater(int value)
  {
      progressBar1.Value += value;
  }
  Here we update the progress bar value. If you don't have a progress bar, same as above.
  
- public void errorHandler(Exception exc)
  In here we handle the exceptions thrown by the api. You can do whatever you want with it
  
To call the api, you have to add the reference to IMDBDLL and create an instance of IMDbManager.
After creating that instance, you must define some properties, like this:

  IMDbManager manag = new IMDbManager();
  manag.parentErrorCaller = formErrorCaller;
  manag.parentFunctionCaller = formFunctionCaller;
  manag.parentProgressUpdaterCaller = formProgressCaller;
  manag.parentProgressConfCaller = formProgressConfCaller;
  
then, you must call it like this: 

  manag.IMDbSearch(int searchMode, string text, int media, int nActors, int sSeas, int eSeas, bool[] fields);
  
where:
  - searchmode must be a 0 or a 1, to define if you want to query imdb by the title of the movie/TV serie or by an ID;
  - text is the query;
  - media is an integer 0 or 1, that defines if you want a movie or a tv serie;
  - nActors is an integer between -1 and 15, that defines how many actors you want to be parsed (-1 represents all);
  - sSeas is ant integer that defines the first season you want to start parsing to get episodes's info of a tv serie;
  - eSeas represents the last season you want to parse;
	- if sSeas or eSeas is equal to -1, than all the seasons are parsed;
  - fields is an array of bools, with 11 elements, each for a diferent field to be parsed.
	[0] - title;
	[1] - year;
	[2] - cover url;
	[3] - user rating;
	[4] - director/creator;
	[5] - seasons (episodes info of a tv serie);
	[6] - genres;
	[7] - tagline;
	[8] - plot;
	[9] - cast (info on actors);
	[10] - runtime;
	
	
If you want to support this and future projects, don't forget to donate to my paypal account.
More info: 
	http://code.google.com/p/imdb-api/
	jpmassena@gmail.com
	