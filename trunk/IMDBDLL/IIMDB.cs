using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace IMDBDLL
{
    public interface IIMDb
    {
        bool searchByTitle(string title);
        bool searchByID(string ID);
        int getType(int tipo);
        ArrayList parseSearch();
        void parseTitlePage();
        Title getTitle();
    }

    public interface IIMDbT
    {
        void parseTitlePage();
        Title getTitle();
    }
}
