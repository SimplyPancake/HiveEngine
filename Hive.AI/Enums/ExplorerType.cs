using System;

namespace Hive.AI.Enums;

public enum ExplorerType
{
	AlphaBeta,
	Minimax,
	MCTS, // Monte Carlo Tree 
	Random,
	GeneticAlgorithm,
	NeuralNetwork,
	SimulatedAnnealing,
	DepthFirst,
	BreadthFirst,
	IterativeDeepening,
	Beam
}
