﻿using System.IO;

namespace AoC_2018
{
    public abstract class BaseProblem
    {
        protected string TypeName => GetType().Name;

        protected string ProblemIndex => TypeName.Substring(TypeName.IndexOf("Problem") + "Problem".Length).TrimStart('0');

        public string FilePath => Path.Combine("Inputs", ProblemIndex + ".in");
    }
}
