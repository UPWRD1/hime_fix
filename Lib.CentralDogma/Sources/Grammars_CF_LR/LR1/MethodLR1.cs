using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    internal class MethodLR1 : ParserGenerator
    {
        internal MethodLR1(Reporting.Reporter reporter) : base("LR(1)", reporter)
		{
		}

		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataLRk(this.reporter, grammar, this.graph);
		}
		
		// TODO: try to remove static methods
        internal static Graph ConstructGraph(CFGrammar grammar)
        {
            // Create the first set
            CFVariable AxiomVar = grammar.GetCFVariable("_Axiom_");
            ItemLR1 AxiomItem = new ItemLR1(AxiomVar.CFRules[0], 0, Epsilon.Instance);
            StateKernel AxiomKernel = new StateKernel();
            AxiomKernel.AddItem(AxiomItem);
            State AxiomSet = AxiomKernel.GetClosure();
            Graph graph = new Graph(AxiomSet);
            // Construct the graph
            foreach (State Set in graph.States)
			{
                Set.BuildReductions(new StateReductionsLR1());
			}
            return graph;
        }
    }
}
