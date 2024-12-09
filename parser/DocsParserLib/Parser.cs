
using DocumentFormat.OpenXml.Wordprocessing;
using ProjectAlfa.parser.DocsParserLib.Abstractions;


namespace ProjectAlfa.parser.DocsParserLib
{
    
    /// <summary>
    /// Класс, представляющий парсер компетенций
    /// </summary>
    public class CompetentionParser : Parser<Competention>
    {
        /// <inheritdoc/>
        public override string[] Filters { get; set; }

        /// <inheritdoc/>
        public override List<Competention>? Data
        {
            get
            {
                if (compets.Count > 0)
                    return compets;

                return null;
            }
        }

        private List<Competention> compets;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="CompetentionParser"/>
        /// </summary>
        /// <param name="document">Экземпляр класса <see cref="DocumentFormat.OpenXml.Wordprocessing.Document"/>, обозсв начающий документ, из которого нужно спарсить компетенции</param>
        public CompetentionParser(Document document) : base(document)
        {
            compets = new List<Competention>();
            Filters = new string[] { "ПЕРЕЧЕНЬ", "ПЛАНИРУЕМЫХ", "РЕЗУЛЬТАТОВ" };
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="CompetentionParser"/>
        /// </summary>
        /// <param name="filename">Путь и название файла, из которого нужно спарсить компетенции</param>
        public CompetentionParser(string filename) : this(new Document(filename))
        { }

        /// <inheritdoc/>
        public override List<Competention>? Parse()
        {
            Table? table = FindTableByTitle(Filters);

            if (table is null)
                return null;

            IEnumerable<TableRow> competetion_table_rows = table.Elements<TableRow>();

            int rows_count = competetion_table_rows.Count();
            IEnumerable<TableRow> rowsWithoutTitle = competetion_table_rows.Skip(1);

            for (int i = 1; i < rows_count / 2 + 1; i++)
            {
                var row = rowsWithoutTitle.Take(3);
                rowsWithoutTitle = rowsWithoutTitle.Except(row);

                Competention? competetion = CreateCompetention(row);

                if (competetion is not null)
                    compets.Add((Competention)competetion);
            }

            compets = compets.GroupBy(n => n.Number).Select(g => g.First()).ToList();
            return compets;
        }

        public Competention? GetCompetentionByName(string name)
        {
             return compets.First(n => n.Name.Equals(name.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private Competention? CreateCompetention(IEnumerable<TableRow> row)
        {
            Competention result = new Competention();
            EvalulationMaterial eval_mat = new EvalulationMaterial();

            IEnumerable<TableCell[]> row_elements = row.Select((elem) => elem.Elements<TableCell>().ToArray());

            int iter = 0;
            foreach (TableCell[] item in row_elements)
            {
                foreach (TableCell cell in item)
                {
                    string text = cell.Elements<Paragraph>().Select(n => n.InnerText).Aggregate((x, y) => $"{x}\n{y}");

                    if (text != "")
                    {
                        switch (iter)
                        {
                            case 0:
                                result.Number = int.Parse(text.Trim());
                                break;

                            case 1:
                                result.Name = text.Trim();
                                break;
                            case 2:
                                string[] sp_text = text.Split('\n');

                                eval_mat.Name = sp_text[0].Trim();
                                eval_mat.Description = sp_text[1].Trim();
                                break;
                            case 3:
                                eval_mat.EM_Type = text.Trim();
                                break;
                            default:
                                eval_mat.EvalulationIndicator = text.Trim();
                                result.EvalulationMaterial.Add(eval_mat);
                                break;
                        }

                        iter++;
                    }

                }

                iter = 2;
            }

            if (result.Name == "")
                return null;

            return result;
        }

        
    }

    /// <summary>
    /// Класс, представляющий парсер вопросов
    /// </summary>
    public class QuestionParser : Parser<Question>
    {
        /// <inheritdoc/>
        public override string[] Filters { get; set; }
        /// <inheritdoc/>
        public override List<Question>? Data 
        {
            get
            {
                if (_questions.Count() > 0)
                    return _questions;

                return null;
            }
        }

        private CompetentionParser _comp_parser;
        private List<Question> _questions;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="QuestionParser"></see>
        /// </summary>
        /// <param name="document">Документ, из которого нужно спарсить данные</param>
        /// <param name="comp_parser">Парсер компетенций</param>
        public QuestionParser(Document document, CompetentionParser comp_parser) :base(document)
        {
            if (comp_parser.Data is null)
                comp_parser.Parse();

            _comp_parser = comp_parser;
            _questions = new List<Question>();
            Filters = new string[]{ "результатов", "компетенций", "вопросы" };
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="QuestionParser">
        /// </summary>
        /// <param name="filename">Путь к документу, из которого нужно спарсить данные</param>
        /// <param name="comp_parser">Парсер компетенций</param>
        public QuestionParser(string filename, CompetentionParser comp_parser) : this(new Document(filename), comp_parser)
        {}

        /// <inheritdoc/>
        public override List<Question>? Parse()
        {
            return ReadTable<Question>(Filters, (question_table, questions) =>
            {
                IEnumerable<TableRow> table_rows = question_table.Elements<TableRow>();
                IEnumerable<TableCell> questions_cells = table_rows.Skip(1).SelectMany(n => n.Elements<TableCell>());

                string title = table_rows.ElementAt(0).InnerText.Trim();
                string? title_match = GetCompetitionNameStr(title);

                Competention? comp = _comp_parser.GetCompetentionByName(title_match);

                ReadQuestions(questions, questions_cells, comp ?? new Competention());
                _questions = questions;
            });

        }

        private void ReadQuestions(List<Question> questions_list, IEnumerable<TableCell> questions_cells, Competention competention)
        {
            foreach (var item in questions_cells)
            {
                int question_num = 1;
                foreach (var item1 in item.Elements<Paragraph>())
                {
                    string question_description = item1.InnerText;

                    if (question_description.Length > 0)
                    {
                        Question question = new Question(question_num, competention, question_description);
                        questions_list.Add(question);

                        question_num++;
                    }
                }

            }
        }
    }

    /// <summary>
    /// Класс, представляющий парсер практических заданий
    /// </summary>
    public class PracticTasksParser : Parser<PracticTask>
    {
        /// <inheritdoc/>
        public override string[] Filters { get; set; }
        /// <inheritdoc/>
        public override List<PracticTask>? Data 
        { 
            get
            {
                if (_practicTasks.Count() > 0)
                    return _practicTasks;

                return null;
            }
        }

        private List<PracticTask> _practicTasks;
        private CompetentionParser _comp_parser;

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="PracticTasksParser"></see>
        /// </summary>
        /// <param name="document">Документ, из которого нужно спарсить данные</param>
        /// <param name="comp_parser">Парсер компетенций</param>
        public PracticTasksParser(Document document, CompetentionParser comp_parser) : base(document)
        {
            if (comp_parser.Data is null)
                comp_parser.Parse();

            _comp_parser = comp_parser;
            _practicTasks = new List<PracticTask>();

            Filters = ["Практические", "задания", "результатов"];
        }

        /// <summary>
        /// Инициализирует экземпляр класса <see cref="PracticTasksParser"></see>
        /// </summary>
        /// <param name="filename">Путь к документу, из которого нужно спарсить данные</param>
        /// <param name="comp_parser">Парсер компетенций</param>
        public PracticTasksParser(string filename, CompetentionParser comp_parser) : this(new Document(filename), comp_parser)
        {}

        /// <inheritdoc/>
        public override List<PracticTask>? Parse()
        { 
            var result_tasks = ReadTable<PracticTask>(Filters,
                (question_table, tasks) => {
                    List<TableRow> tasks_rows = question_table.Elements<TableRow>().ToList();

                    int question_num = 1;
                    Competention? comp = null;
                    foreach (TableRow row in tasks_rows)
                    {
                        string? title = GetCompetitionNameStr(row.InnerText);

                        if (title is not null)
                        {
                            comp = _comp_parser.GetCompetentionByName(title);
                            question_num = 1;
                            continue;
                        }

                        tasks.Add(PracticeTaskRowParse(row, question_num, comp ?? new Competention()));
                        question_num++;
                    }
                });

            _practicTasks = result_tasks;
            return result_tasks;
        }

        private PracticTask PracticeTaskRowParse(TableRow row, int question_num, Competention comp)
        {
            IEnumerable<Paragraph> answer = row.ElementAt(1).Elements<Paragraph>();

            string title = answer.ElementAt(0).InnerText.Trim();
            List<AnswerVariant> variants = new List<AnswerVariant>();

            int i = 0;
            foreach (var paragraph in answer.Skip(1))
            {
                string ans_description = paragraph.InnerText.Trim();
                bool valid_var = false;

                if (ans_description != "")
                {
                    if (paragraph.Elements<Run>().Any(n => n.RunProperties?.Bold is not null))
                        valid_var = true;
                
                    variants.Add(new AnswerVariant(i, ans_description, valid_var));
                    i++;
                }
            }

            PracticTask task = new PracticTask(question_num, comp, title, variants);
            return task;
        }
    }
}
