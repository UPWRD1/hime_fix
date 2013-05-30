using System;
using System.Reflection;
using Hime.CentralDogma;
using System.IO;

namespace Hime.Benchmark
{
    class Benchmark
    {
        private string language;
        private string input;
        private string output;
        private int sampleFactor;
        private int expCount;
        private bool rebuildInput;
        private bool rebuildParsers;
        private bool doStats;
        private bool doLexer;
        private bool doParserLALR;
        private bool doParserRNGLR;

        public Benchmark()
        {
            SetupProfileFull();
        }

        private void SetupProfileFull()
        {
            this.language = "CSharp4.gram";
            this.input = "Perf.gram";
            this.output = "result.txt";
            this.sampleFactor = 600;
            this.expCount = 50;
            this.rebuildInput = true;
            this.rebuildParsers = true;
            this.doStats = true;
            this.doLexer = true;
            this.doParserLALR = true;
            this.doParserRNGLR = false;
        }

        private void SetupProfilePerfLexer()
        {
            this.language = "CSharp4.gram";
            this.input = "Perf.gram";
            this.output = "result.txt";
            this.sampleFactor = 600;
            this.expCount = 20;
            this.rebuildInput = false;
            this.rebuildParsers = false;
            this.doStats = false;
            this.doLexer = true;
            this.doParserLALR = false;
            this.doParserRNGLR = false;
        }

        private void SetupProfilePerfLALR()
        {
            this.language = "CSharp4.gram";
            this.input = "Perf.gram";
            this.output = "result.txt";
            this.sampleFactor = 600;
            this.expCount = 20;
            this.rebuildInput = true;
            this.rebuildParsers = false;
            this.doStats = false;
            this.doLexer = false;
            this.doParserLALR = true;
            this.doParserRNGLR = false;
        }

        public void Run()
        {
            if (System.IO.File.Exists(output))
                System.IO.File.Delete(output);
            
            if (rebuildInput)
                BuildInput();
            
            Assembly asmLALR = null;
            Assembly asmGLR = null;
            if (rebuildParsers)
            {
                asmLALR = Compile(ParsingMethod.LALR1);
                asmGLR = Compile(ParsingMethod.RNGLALR1);
            }
            else
            {
                asmLALR = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "gen_LALR1.dll"));
                asmGLR = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "gen_RNGLALR1.dll"));
            }

            if (doStats)
                OutputInputStats(asmLALR);

            if (doLexer)
            {
                System.IO.File.AppendAllText(output, "-- lexer\n");
                Console.WriteLine("-- lexer");
                for (int i = 0; i != expCount; i++)
                    BenchmarkLexer(asmLALR, i);
            }

            if (doParserLALR)
            {
                System.IO.File.AppendAllText(output, "-- parser LALR\n");
                Console.WriteLine("-- parser LALR");
                for (int i = 0; i != expCount; i++)
                    BenchmarkParser(asmLALR, i);
            }

            if (doParserRNGLR)
            {
                System.IO.File.AppendAllText(output, "-- parser GLR\n");
                Console.WriteLine("-- parser GLR");
                for (int i = 0; i != expCount; i++)
                    BenchmarkParser(asmGLR, i);
            }
        }

        private void BuildInput()
        {
            System.IO.Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hime.Benchmark.Languages." + language);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Close();
            if (System.IO.File.Exists(input))
                System.IO.File.Delete(input);
            for (int i = 0; i != sampleFactor; i++)
                System.IO.File.AppendAllText(input, content);
        }

        private Assembly Compile(ParsingMethod method)
        {
            System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputRaw(stream);
            task.Namespace = "Hime.Benchmark.Generated";
            task.GrammarName = "FileCentralDogma";
            task.CodeAccess = AccessModifier.Public;
            task.Method = method;
            task.OutputPrefix = "gen_" + method.ToString();
            task.Execute();
            return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "gen_" + method.ToString() + ".dll"));
        }

        private void OutputInputStats(Assembly assembly)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(assembly, reader);
            Hime.Redist.Symbols.Token token = lexer.GetNextToken();
            int count = 0;
            while (token.SymbolID != 1)
            {
                token = lexer.GetNextToken();
                count++;
            }
            reader.Close();
            System.GC.Collect();
            System.IO.File.AppendAllText(output, "-- tokens: " + count + "\n");
            Console.WriteLine("-- tokens: " + count);
        }

        private Hime.Redist.Lexer.TextLexer GetLexer(Assembly assembly, System.IO.StreamReader reader)
        {
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Hime.Redist.Lexer.TextLexer;
        }

        private Hime.Redist.Parsers.BaseLRParser GetParser(Assembly assembly, System.IO.StreamReader reader)
        {
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(assembly, reader);
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            Type parserType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaParser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as Hime.Redist.Parsers.BaseLRParser;
        }

        private void BenchmarkLexer(Assembly assembly, int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(assembly, reader);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            Hime.Redist.Symbols.Token token = lexer.GetNextToken();
            while (token.SymbolID != 1)
                token = lexer.GetNextToken();
            watch.Stop();
            reader.Close();
            System.GC.Collect();
            System.IO.File.AppendAllText(output, watch.ElapsedMilliseconds + "\n");
            Console.WriteLine(watch.ElapsedMilliseconds);
        }

        private void BenchmarkParser(Assembly assembly, int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Hime.Redist.Parsers.BaseLRParser parser = GetParser(assembly, reader);
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            parser.Parse();
            watch.Stop();
            reader.Close();
            System.GC.Collect();
            System.IO.File.AppendAllText(output, watch.ElapsedMilliseconds + "\n");
            Console.WriteLine(watch.ElapsedMilliseconds);
        }
    }
}
