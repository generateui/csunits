// Copyright (c) 2011 Anders Gustafsson, Cureos AB.
// All rights reserved. This software and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html

using System;

#if SINGLE
using AmountType = System.Single;
#elif DECIMAL
using AmountType = System.Decimal;
#elif DOUBLE
using AmountType = System.Double;
#endif

namespace Cureos.Measures
{
	/// <summary>
	/// Interface representing a scalar measure of a specific quantity
	/// </summary>
	/// <typeparam name="Q">Measured quantity</typeparam>
	public interface IMeasure<Q> where Q : struct, IQuantity<Q>
	{
		/// <summary>
		/// Gets the measured amount in the <see cref="Unit">current unit of measure</see>
		/// </summary>
		AmountType Amount
		{
			get;
		}

		/// <summary>
		/// Gets the unit of measure
		/// </summary>
		IUnit<Q> Unit { get; }

		/// <summary>
		/// Gets the amount of this measure in the requested unit
		/// </summary>
		/// <param name="iUnit">Unit to which the measured amount should be converted</param>
		/// <returns>Measured amount converted into <paramref name="iUnit">specified unit</paramref></returns>
		AmountType GetAmount(IUnit<Q> iUnit);
	}
}

