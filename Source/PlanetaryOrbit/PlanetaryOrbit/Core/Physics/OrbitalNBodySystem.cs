using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetaryOrbit.Core.Physics.Integrators;

namespace PlanetaryOrbit.Core.Physics.Newtonian
{
    public class OrbitalNBodySystem
    {
        List<OrbitalBody> mBodies;

        private int dimension;
        private double[] q;
        private double[] qError;
        private double[] qPredicted;
        private double[] qPredictedError;
        private double t;
        private double tError;
        private double tPredicted;
        private double tPredictedError;
        private double[] v;
        private double[] vError;
        private double[] vPredicted;
        private double[] vPredictedError;

        public OrbitalNBodySystem(ref List<OrbitalBody> _list_bodies, double _t0, double _tError = 0)
        {
            this.mBodies = _list_bodies;
            dimension = this.mBodies.Count * 3;
            q = new double[dimension];
            v = new double[dimension];
            qError = new double[dimension];
            vError = new double[dimension];
            t = _t0;
            this.tError = _tError;
            updateState();
        }

        public void AdvancePredictions(double tmax, double maxTimestep, int samplingPeriod)
        {
            // Make sure we're predicting at least a point.
            if (samplingPeriod * maxTimestep > tmax - tPredicted) { return; }
            SymplecticPRK.Solution solution = Integrators.SymplecticPRK.IncrementSPRK(
                          computeForce: computeAccelerations,
                          computeVelocity: computeVelocities,
                          q0: qPredicted, p0: vPredicted, t0: tPredicted,
                          tmax: tmax, Δt: maxTimestep,
                          coefficients: SymplecticPRK.Order5Optimal,
                          samplingPeriod: samplingPeriod,
                          qError: qPredictedError,
                          pError: vPredictedError,
                          tError: tPredictedError);
            qPredicted = solution.position.Last();
            vPredicted = solution.momentum.Last();
            tPredicted = solution.time.Last();
            qPredictedError = solution.positionError;
            vPredictedError = solution.momentumError;
            tPredictedError = solution.timeError;

            for (int i = 1; i < solution.position.Length; ++i)
            {
                for (int b = 0; b < this.mBodies.Count; ++b)
                {
                    this.mBodies[b].mPredictedTrajectory.Add(new Event
                    {
                        q = new Vector3D
                        {
                            X = solution.position[i][3 * b],
                            Y = solution.position[i][3 * b + 1],
                            Z = solution.position[i][3 * b + 2]
                        },
                        t = solution.time[i]
                    });
                }
            }
        }
        public void Evolve(double tmax, double maxTimestep)
        {
            SymplecticPRK.Solution solution = Integrators.SymplecticPRK.IncrementSPRK(
                  computeForce: computeAccelerations,
                  computeVelocity: computeVelocities,
                  q0: q, p0: v, t0: t, tmax: tmax,
                  Δt: (tmax - t) / Math.Ceiling((tmax - t) / maxTimestep),
                  coefficients: SymplecticPRK.Order5Optimal,
                  samplingPeriod: 0, qError: qError, pError: vError, tError: tError);
            q = solution.position[0];
            v = solution.momentum[0];
            t = solution.time[0];
            qError = solution.positionError;
            vError = solution.momentumError;
            tError = solution.timeError;
            updateBodies();
        }

        public void RecalculatePredictions(double tmax, double maxTimestep, int samplingPeriod)
        {
            qPredicted = (double[])q.Clone();
            qPredictedError = (double[])qError.Clone();
            vPredicted = (double[])v.Clone();
            vPredictedError = (double[])vError.Clone();
            tPredicted = t;
            tPredictedError = tError;
            for (int b = 0; b < this.mBodies.Count; ++b)
            {
                this.mBodies[b].mPredictedTrajectory.Clear();
            }
            AdvancePredictions(tmax, maxTimestep, samplingPeriod);
        }

        private void computeAccelerations(double[] q, double t, ref double[] result)
        {
            for (int k = 0; k < dimension; ++k)
            {
                result[k] = 0;
            }

            for (int b1 = 0; b1 < this.mBodies.Count; ++b1)
            {
                for (int b2 = b1 + 1; b2 < this.mBodies.Count; ++b2)
                {
                    if (!(this.mBodies[b1].IsMassless && this.mBodies[b2].IsMassless))
                    {
                        double Δq0 = q[3 * b1] - q[3 * b2];
                        double Δq1 = q[3 * b1 + 1] - q[3 * b2 + 1];
                        double Δq2 = q[3 * b1 + 2] - q[3 * b2 + 2];

                        double squaredDistance = Δq0 * Δq0 + Δq1 * Δq1 + Δq2 * Δq2;
                        double denominator = squaredDistance * Math.Sqrt(squaredDistance);

                        if (!this.mBodies[b2].IsMassless)
                        {
                            double μ2OverRSquared = this.mBodies[b2].mMU / denominator;
                            result[3 * b1] -= Δq0 * μ2OverRSquared;
                            result[3 * b1 + 1] -= Δq1 * μ2OverRSquared;
                            result[3 * b1 + 2] -= Δq2 * μ2OverRSquared;
                        }

                        if (!this.mBodies[b1].IsMassless)
                        {
                            double μ1OverRSquared = this.mBodies[b1].mMU / denominator;
                            result[3 * b2] += Δq0 * μ1OverRSquared;
                            result[3 * b2 + 1] += Δq1 * μ1OverRSquared;
                            result[3 * b2 + 2] += Δq2 * μ1OverRSquared;
                        }
                    }
                }
            }
        }
        private void computeVelocities(double[] v, ref double[] result)
        {
            result = v;
        }
        private void updateBodies()
        {
            for (int b = 0; b < this.mBodies.Count; ++b)
            {
                this.mBodies[b].sCurrent.position.X = q[3 * b];
                this.mBodies[b].sCurrent.position.Y = q[3 * b + 1];
                this.mBodies[b].sCurrent.position.Z = q[3 * b + 2];
                this.mBodies[b].sCurrent.velocity.X = v[3 * b];
                this.mBodies[b].sCurrent.velocity.Y = v[3 * b + 1];
                this.mBodies[b].sCurrent.velocity.Z = v[3 * b + 2];
                this.mBodies[b].qError.X = qError[3 * b];
                this.mBodies[b].qError.Y = qError[3 * b + 1];
                this.mBodies[b].qError.Z = qError[3 * b + 2];
                this.mBodies[b].vError.X = vError[3 * b];
                this.mBodies[b].vError.Y = vError[3 * b + 1];
                this.mBodies[b].vError.Z = vError[3 * b + 2];
            }
        }

        private void updateState()
        {
            for (int b = 0; b < this.mBodies.Count; ++b)
            {
                q[3 * b] = this.mBodies[b].sCurrent.position.X;
                q[3 * b + 1] = this.mBodies[b].sCurrent.position.Y;
                q[3 * b + 2] = this.mBodies[b].sCurrent.position.Z;
                v[3 * b] = this.mBodies[b].sCurrent.velocity.X;
                v[3 * b + 1] = this.mBodies[b].sCurrent.velocity.Y;
                v[3 * b + 2] = this.mBodies[b].sCurrent.velocity.Z;
                qError[3 * b] = this.mBodies[b].qError.X;
                qError[3 * b + 1] = this.mBodies[b].qError.Y;
                qError[3 * b + 2] = this.mBodies[b].qError.Z;
                vError[3 * b] = this.mBodies[b].vError.X;
                vError[3 * b + 1] = this.mBodies[b].vError.Y;
                vError[3 * b + 2] = this.mBodies[b].vError.Z;
            }
        }
    }
}
