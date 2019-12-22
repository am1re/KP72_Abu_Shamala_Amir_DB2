using System;
using lab2.Database;
using lab2.Database.DAO;
using lab2.Models;
using lab2.Views;
using lab2.Views.CrudViews;

namespace lab2.Controllers
{
    public class Controller
    {
        private readonly DAO<Accident> _accidentDAO;
        private readonly DAO<Car> _carDAO;
        private readonly DAO<Person> _personDAO;
        private readonly FullTextSearch _fullTextSearch;

        public Controller(DbConnection dbConnection)
        {
            _accidentDAO = new AccidentDAO(dbConnection);
            _carDAO = new CarDAO(dbConnection);
            _personDAO = new PersonDAO(dbConnection);
            _fullTextSearch = new FullTextSearch(dbConnection);
        }

        public void Begin()
        {
            while (true)
            {
                var menuCom = MenuView.ShowMain();
                if (menuCom == MenuCommands.Exit)
                    break;
                if (menuCom == MenuCommands.Crud)
                    BeginCrud();
                if (menuCom == MenuCommands.Random)
                    ExecuteRandomise();
                if (menuCom == MenuCommands.FullTextSearch)
                    ExecuteFullTestSearch();
            }
        }

        private void BeginCrud()
        {
            while (true)
            {
                var entity = MenuView.ShowEntities();
                if (entity == Entities.None)
                    break;
                if (entity == Entities.Accident)
                    ExecuteCrud(new AccidentView(_personDAO), _accidentDAO);
                if (entity == Entities.Car)
                    ExecuteCrud(new CarView(), _carDAO);
                if (entity == Entities.Person)
                    ExecuteCrud(new PersonView(_carDAO), _personDAO);
            }
        }

        private void ExecuteCrud<T>(CrudView<T> view, DAO<T> dao)
        {
            var page = 0;
            while (true)
            {
                var com = view.Begin(dao.Get(page), page);
                if (com == CrudOperations.None)
                    break;
                if (com == CrudOperations.PageLeft && page > 0)
                    page--;
                if (com == CrudOperations.PageRight)
                    page++;
                try
                {
                    if (com == CrudOperations.Create)
                        dao.Create(view.Create());
                    if (com == CrudOperations.Read)
                        view.ShowReadResult(dao.Get(view.Read()));
                    if (com == CrudOperations.Update)
                        dao.Update(view.Update(dao.Get(view.Read())));
                    if (com == CrudOperations.Delete)
                        dao.Delete(view.Read());
                    if (com == CrudOperations.Search)
                        ExecuteSearch(view, dao);
                    if (com == CrudOperations.Create || com == CrudOperations.Delete
                                                     || com == CrudOperations.Update)
                        view.OperationStatusOutput(true);
                }
                catch(Exception e)
                {
                    view.OperationStatusOutput(false);
                }
            }
        }

        private void ExecuteSearch<T>(CrudView<T> view, DAO<T> dao)
        {
            var type = typeof(T);
            if (type == typeof(Person))
            {
                var v = view as PersonView;
                var d = dao as PersonDAO;
                v.ShowReadResult(d.Search(v.Search()));
            }
            if (type == typeof(Accident))
            {
                var v = view as AccidentView;
                var d = dao as AccidentDAO;
                var q = v.Search();
                v.ShowReadResult(d.Search(q.str, q.from, q.to));
            }
            if (type == typeof(Car))
            {
                var v = view as CarView;
                var d = dao as CarDAO;
                v.ShowReadResult(d.Search(v.Search()));
            }
        }

        private void ExecuteRandomise()
        {
            var number = RandomiseView.ShowRandomise();
            if (number == -1)
                return;
            var randomiser = new Randomiser(_accidentDAO, _carDAO, _personDAO);
            randomiser.Randomise(number);
        }

        private void ExecuteFullTestSearch()
        {
            var command = FullTextSearchView.Begin();
            FullTextSearchView.ShowResults(command.Item1 == FullTextSearchCommands.FullPhrase
                ? _fullTextSearch.GetFullPhrase("\"record-number\"", "location", "accident", command.Item2)
                : _fullTextSearch.GetAllWithIncludedWord("\"record-number\"", "location", "accident", command.Item2));
        }
    }
}