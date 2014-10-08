using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POSystem;

using System.Threading.Tasks;

namespace PlanetaryOrbit.Core.Physics
{
    public static class OrbitalMechanics
    {
        private static List<State> k1, k2, k3, k4, k5, k6, k7, sn5;

        public static Vector3D CalculateGravForce(OrbitalBody _p1, OrbitalBody _p2)
        {
            Vector3D deltaVector = _p2.sCurrent.position - _p1.sCurrent.position;
            double magnitude = Math.Sqrt(Vector3D.Dot(deltaVector, deltaVector));
            double force = (GameSettings.PHYSICS_GRAVITY_CONSTANT * _p1.mMass * _p2.mMass) / (magnitude * magnitude);
            return (deltaVector * (1.0 / magnitude)) * force;
        }

        public static double CalculateVelocity(double _distance, double _parentMass)
        {
            return Math.Sqrt((GameSettings.PHYSICS_GRAVITY_CONSTANT * _parentMass) / (_distance * 1000));
        }

        public static void StartRKF45(ref List<OrbitalBody> _list_planets)
        {
            k1 = new List<State>();
            k2 = new List<State>();
            k3 = new List<State>();
            k4 = new List<State>();
            k5 = new List<State>();
            k6 = new List<State>();
            k7 = new List<State>();
            sn5 = new List<State>();

            foreach (OrbitalBody localBody in _list_planets)
            {
                k1.Add(new State());
                k2.Add(new State());
                k3.Add(new State());
                k4.Add(new State());
                k5.Add(new State());
                k6.Add(new State());
                k7.Add(new State());
                sn5.Add(new State());
            }
        }

        public static void StepSym(ref List<OrbitalBody> _list_planets, double dT)
        {
            foreach (OrbitalBody localBody in _list_planets)
                localBody.SYM_kick_noforce(dT / 4);

            foreach (OrbitalBody localBody in _list_planets)
                localBody.SYM_drift(dT / 2);

            foreach (OrbitalBody localBody in _list_planets)
                localBody.sLast = localBody.sCurrent;

            foreach (OrbitalBody localBody in _list_planets)
                localBody.SYM_kick(ref _list_planets, (dT / 2));

            foreach (OrbitalBody localBody in _list_planets)
                localBody.SYM_drift(dT / 2);

            foreach (OrbitalBody localBody in _list_planets)
                localBody.sLast = localBody.sCurrent;

            foreach (OrbitalBody localBody in _list_planets)
                localBody.SYM_kick(ref _list_planets, (dT / 4));

        }

        public static void StepRKF45(ref List<OrbitalBody> _list_planets, double dT, double absTol, double relTol)
        {
            State sn4;
            const double A21 = 0.2, A31 = 0.075, A32 = 0.225, A41 = 44.0 / 45.0;
            const double A42 = -56.0 / 15.0, A43 = 32.0 / 9.0, A51 = 19372.0 / 6561.0;
            const double A52 = -25360.0 / 2187.0, A53 = 64448.0 / 6561.0, A54 = -212.0 / 729.0;
            const double A61 = 9017.0 / 3168.0, A62 = -355.0 / 33.0, A63 = 46732.0 / 5247.0;
            const double A64 = 49.0 / 176.0, A65 = -5103.0 / 18656.0;
            const double B11 = 35.0 / 384.0, B13 = 500.0 / 1113.0, B14 = 125.0 / 192.0;
            const double B15 = -2187.0 / 6784.0, B16 = 11.0 / 84.0;
            const double B21 = 5179.0 / 57600.0, B23 = 7571.0 / 16695.0, B24 = 393.0 / 640.0;
            const double B25 = -92097.0 / 339200.0, B26 = 187.0 / 2100.0, B27 = 0.025;
            double t1, t2, t3, t4, t5, t6, t7;
            Vector3D e;
            double err, maxerr = 0.0;

            foreach (OrbitalBody localBody in _list_planets)
                localBody.sTmp = localBody.sLast;

            CalculateRHS(ref _list_planets);

            t1 = A21 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k1[i] = _list_planets[i].sRHS;

                _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position;
                _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity;
            }

            CalculateRHS(ref _list_planets);

            t1 = A31 * dT;
            t2 = A32 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k2[i] = _list_planets[i].sRHS;

                _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position;
                _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity;
            }

            CalculateRHS(ref _list_planets);

            t1 = A41 * dT;
            t2 = A42 * dT;
            t3 = A43 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k3[i] = _list_planets[i].sRHS;

                _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position + t3 * k3[i].position;
                _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity + t3 * k3[i].velocity;
            }

            CalculateRHS(ref _list_planets);

            t1 = A51 * dT;
            t2 = A52 * dT;
            t3 = A53 * dT;
            t4 = A54 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k4[i] = _list_planets[i].sRHS;

                _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position + t3 * k3[i].position + t4 * k4[i].position;
                _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity + t3 * k3[i].velocity + t4 * k4[i].velocity;
            }

            CalculateRHS(ref _list_planets);

            t1 = A61 * dT;
            t2 = A62 * dT;
            t3 = A63 * dT;
            t4 = A64 * dT;
            t5 = A65 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k5[i] = _list_planets[i].sRHS;

                _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position + t3 * k3[i].position + t4 * k4[i].position + t5 * k5[i].position;
                _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity + t3 * k3[i].velocity + t4 * k4[i].velocity + t5 * k5[i].velocity;
            }

            CalculateRHS(ref _list_planets);

            t1 = B11 * dT;
            t3 = B13 * dT;
            t4 = B14 * dT;
            t5 = B15 * dT;
            t6 = B16 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k6[i] = _list_planets[i].sRHS;
                State tmpState = new State();
                tmpState.position = _list_planets[i].sLast.position + t1 * k1[i].position + t3 * k3[i].position + t4 * k4[i].position + t5 * k5[i].position + t6 * k6[i].position;
                tmpState.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t3 * k3[i].velocity + t4 * k4[i].velocity + t5 * k5[i].velocity + t6 * k6[i].velocity;
                sn5[i] = tmpState;

                _list_planets[i].sTmp = sn5[i];
            }

            CalculateRHS(ref _list_planets);

            t1 = B21 * dT;
            t3 = B23 * dT;
            t4 = B24 * dT;
            t5 = B25 * dT;
            t6 = B26 * dT;
            t7 = B27 * dT;
            for (int i = 0; i < _list_planets.Count; i++)
            {
                k7[i] = _list_planets[i].sRHS;

                sn4.position = _list_planets[i].sLast.position + t1 * k1[i].position + t3 * k3[i].position + t4 * k4[i].position + t5 * k5[i].position + t6 * k6[i].position + t7 * k7[i].position;

                e = sn5[i].position - sn4.position;
                err = Math.Sqrt(e.X * e.X + e.Y * e.Y + e.Z * e.Z) / (absTol + relTol * Math.Sqrt(sn5[i].position.X * sn5[i].position.X + sn5[i].position.Y * sn5[i].position.Y + sn5[i].position.Z * sn5[i].position.Z));

                if (err > maxerr)
                    maxerr = err;
            }

            if ((maxerr < 1.0) || (dT == 1.0))
            {
                if (maxerr != 0.0)
                    dT = 0.9 * dT * Math.Pow(maxerr, -0.20);
                else
                    dT *= 2.0;
                if (dT < 1.0)
                    dT = 1.0;
            }
            else
            {
                if (maxerr != 0.0)
                    dT = 0.9 * dT * Math.Pow(maxerr, -0.20);
                else
                    dT *= 0.5;
                if (dT < 1.0)
                    dT = 1.0;

                foreach (OrbitalBody localBody in _list_planets)
                    localBody.sTmp = localBody.sLast;

                CalculateRHS(ref _list_planets);

                t1 = A21 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k1[i] = _list_planets[i].sRHS;

                    _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position;
                    _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity;
                }

                CalculateRHS(ref _list_planets);

                t1 = A31 * dT;
                t2 = A32 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k2[i] = _list_planets[i].sRHS;

                    _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position;
                    _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity;
                }

                CalculateRHS(ref _list_planets);

                t1 = A41 * dT;
                t2 = A42 * dT;
                t3 = A43 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k3[i] = _list_planets[i].sRHS;

                    _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position + t3 * k3[i].position;
                    _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity + t3 * k3[i].velocity;
                }

                CalculateRHS(ref _list_planets);

                t1 = A51 * dT;
                t2 = A52 * dT;
                t3 = A53 * dT;
                t4 = A54 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k4[i] = _list_planets[i].sRHS;

                    _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position + t3 * k3[i].position + t4 * k4[i].position;
                    _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity + t3 * k3[i].velocity + t4 * k4[i].velocity;
                }

                CalculateRHS(ref _list_planets);

                t1 = A61 * dT;
                t2 = A62 * dT;
                t3 = A63 * dT;
                t4 = A64 * dT;
                t5 = A65 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k5[i] = _list_planets[i].sRHS;

                    _list_planets[i].sTmp.position = _list_planets[i].sLast.position + t1 * k1[i].position + t2 * k2[i].position + t3 * k3[i].position + t4 * k4[i].position + t5 * k5[i].position;
                    _list_planets[i].sTmp.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t2 * k2[i].velocity + t3 * k3[i].velocity + t4 * k4[i].velocity + t5 * k5[i].velocity;
                }

                CalculateRHS(ref _list_planets);

                t1 = B11 * dT;
                t3 = B13 * dT;
                t4 = B14 * dT;
                t5 = B15 * dT;
                t6 = B16 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k6[i] = _list_planets[i].sRHS;
                    State tmpState = new State();
                    tmpState.position = _list_planets[i].sLast.position + t1 * k1[i].position + t3 * k3[i].position + t4 * k4[i].position + t5 * k5[i].position + t6 * k6[i].position;
                    tmpState.velocity = _list_planets[i].sLast.velocity + t1 * k1[i].velocity + t3 * k3[i].velocity + t4 * k4[i].velocity + t5 * k5[i].velocity + t6 * k6[i].velocity;
                    sn5[i] = tmpState;

                    _list_planets[i].sTmp = sn5[i];
                }

                CalculateRHS(ref _list_planets);

                t1 = B21 * dT;
                t3 = B23 * dT;
                t4 = B24 * dT;
                t5 = B25 * dT;
                t6 = B26 * dT;
                t7 = B27 * dT;
                for (int i = 0; i < _list_planets.Count; i++)
                {
                    k7[i] = _list_planets[i].sRHS;

                    sn4.position = _list_planets[i].sLast.position + t1 * k1[i].position + t3 * k3[i].position + t4 * k4[i].position + t5 * k5[i].position + t6 * k6[i].position + t7 * k7[i].position;

                    e = sn5[i].position - sn4.position;
                    err = Math.Sqrt(e.X * e.X + e.Y * e.Y + e.Z * e.Z) / (absTol + relTol * Math.Sqrt(sn5[i].position.X * sn5[i].position.X + sn5[i].position.Y * sn5[i].position.Y + sn5[i].position.Z * sn5[i].position.Z));

                    if (err > maxerr)
                        maxerr = err;
                }

                if (maxerr != 0.0)
                    dT = 0.9 * dT * Math.Pow(maxerr, -0.20);
                else
                    dT *= 2.0;
                if (dT < 1.0)
                    dT = 1.0;
            }

            for (int i = 0; i < _list_planets.Count; i++)
                _list_planets[i].sCurrent = sn5[i];
        }

        public static State initRKF45(double a, double e, double i, double omega, double Omega, double M0, double mu)
        {
            State _s;
            double E, Ep, c1, c2, p;
            Vector3D t1, t2;
            _s.force = Vector3D.Zero;

            t1 = Vector3D.Zero;
            t2 = Vector3D.Zero;

            i = i * Math.PI / 180.0;
            omega = omega * Math.PI / 180.0;
            Omega = Omega * Math.PI / 180.0;

            t1.X = Math.Cos(Omega) * Math.Cos(omega) - Math.Sin(Omega) * Math.Cos(i) * Math.Sin(omega);
            t1.Z = Math.Sin(Omega) * Math.Cos(omega) + Math.Cos(Omega) * Math.Cos(i) * Math.Sin(omega);
            t1.Y = Math.Sin(i) * Math.Sin(omega);

            t2.X = -Math.Cos(Omega) * Math.Sin(omega) - Math.Sin(Omega) * Math.Cos(i) * Math.Cos(omega);
            t2.Z = -Math.Sin(Omega) * Math.Sin(omega) + Math.Cos(Omega) * Math.Cos(i) * Math.Cos(omega);
            t2.Y = Math.Sin(i) * Math.Cos(omega);

            p = Math.Sqrt(mu / a);

            E = M0;

            do
            {
                Ep = E;
                E = Ep - (Ep - e * Math.Sin(Ep) - M0) / (1.0 - e * Math.Cos(Ep));
            } while (Math.Abs(Ep - E) > 1.0E-15);

            c1 = a * (Math.Cos(E) - e);
            c2 = a * Math.Sqrt(1.0 - e * e) * Math.Sin(E);
            _s.position = new Vector3D((t1.X * c1 + t2.X * c2), (t1.Y * c1 + t2.Y * c2), (t1.Z * c1 + t2.Z * c2));

            c1 = -Math.Sin(E) * p / (1.0 - e * Math.Cos(E));
            c2 = Math.Sqrt(1.0 - e * e) * Math.Cos(E) * p / (1.0 - e * Math.Cos(E));
            _s.velocity = new Vector3D((t1.X * c1 + t2.X * c2), (t1.Y * c1 + t2.Y * c2), (t1.Z * c1 + t2.Z * c2));

            return _s;
        }

        public static double CalculateEnergy(ref List<OrbitalBody> _list_planets)
        {
            double energy = 0.0, ke;
            Vector3D d = Vector3D.Zero;

            for (int i = 0; i < _list_planets.Count; i++)
            {
                ke = _list_planets[i].sCurrent.velocity.X * _list_planets[i].sCurrent.velocity.X + _list_planets[i].sCurrent.velocity.Y * _list_planets[i].sCurrent.velocity.X + _list_planets[i].sCurrent.velocity.Z * _list_planets[i].sCurrent.velocity.Z;
                energy += 0.5 * ke * _list_planets[i].mMU;
            }

            for (int i = 0; i < _list_planets.Count - 1; i++)
            {
                for (int j = i + 1; j < _list_planets.Count; j++)
                {
                    d = _list_planets[i].sCurrent.position - _list_planets[j].sCurrent.position;
                    energy -= _list_planets[i].mMU * _list_planets[j].mMU / Math.Sqrt(d.X * d.X + d.Y * d.Y + d.Z * d.Z);
                }
            }

            return energy;
        }

        public static State CalculateMomentum(ref List<OrbitalBody> _list_planets)
        {
            State _momentum;

            _momentum.position = Vector3D.Zero;
            _momentum.velocity = Vector3D.Zero;
            _momentum.force = Vector3D.Zero;

            for (int i = 0; i < _list_planets.Count; i++)
            {
                _momentum.position += _list_planets[i].mMU * _list_planets[i].sCurrent.velocity;
                Vector3D tmpVector = new Vector3D(_list_planets[i].mMU * (_list_planets[i].sCurrent.position.Y * _list_planets[i].sCurrent.velocity.Z - _list_planets[i].sCurrent.position.Z * _list_planets[i].sCurrent.velocity.Y), _list_planets[i].mMU * (_list_planets[i].sCurrent.position.Z * _list_planets[i].sCurrent.velocity.X - _list_planets[i].sCurrent.position.X * _list_planets[i].sCurrent.velocity.Z), _list_planets[i].mMU * (_list_planets[i].sCurrent.position.X * _list_planets[i].sCurrent.velocity.Y - _list_planets[i].sCurrent.position.Y * _list_planets[i].sCurrent.velocity.X));
                _momentum.velocity += tmpVector;
            }

            return _momentum;
        }

        public static void CalculateRHS(ref List<OrbitalBody> _list_planets)
        {
            double r, tmp;
            Vector3D d;

            foreach (OrbitalBody localBody in _list_planets)
            {
                localBody.sRHS.position = localBody.sTmp.velocity;
                localBody.sRHS.velocity = Vector3D.Zero;
            }

            for (int i = 0; i < _list_planets.Count - 1; i++)
            {
                for (int j = i + 1; j < _list_planets.Count; j++)
                {
                    d = _list_planets[i].sTmp.position - _list_planets[j].sTmp.position;
                    r = 1.0 / d.LengthSquared();
                    r *= Math.Sqrt(r);
                    tmp = _list_planets[j].mMU * r;
                    _list_planets[i].sRHS.velocity -= tmp * d;

                    tmp = _list_planets[i].mMU * r;
                    _list_planets[j].sRHS.velocity += tmp * d;
                }
            }
        }

        public static void AddToSystem(OrbitalBody _body)
        {
            if (_body.mParentObject == null)
            {
                _body.sCurrent.position = Vector3D.Zero;
                _body.sCurrent.velocity = Vector3D.Zero;
            }
            else
            {
                double a, e, i, ap, lan, m0;
                a = _body.mSemiMajorAxis;
                e = _body.mEccentricity;
                i = _body.mInclination;
                ap = _body.mArgPeriapsis;
                lan = _body.mLongOfAscendNode;
                m0 = _body.mMeanAnomAtEpoch;

                State newState = initRKF45(a, e, i, ap, lan, m0, _body.mParentObject.mMU);
                _body.sCurrent.position = newState.position + _body.mParentObject.sCurrent.position;
                _body.sCurrent.velocity = newState.velocity + _body.mParentObject.sCurrent.velocity;
            }
        }
    }
}
