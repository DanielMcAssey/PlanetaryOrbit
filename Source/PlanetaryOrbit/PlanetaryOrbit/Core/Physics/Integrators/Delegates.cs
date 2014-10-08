using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlanetaryOrbit.Core.Physics.Integrators
{
    public delegate void AutonomousRightHandSideComputation(double[] y, ref double[] result);
    public delegate void RightHandSideComputation(double[] y, double t, ref double[] result);
}
