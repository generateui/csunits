// Copyright (c) 2011 Anders Gustafsson, Cureos AB.
// All rights reserved. This software and the accompanying materials
// are made available under the terms of the Eclipse Public License v1.0
// which accompanies this distribution, and is available at
// http://www.eclipse.org/legal/epl-v10.html

using System;
using Cureos.Measures.Quantities;

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
	/// Quantity-typed representation of a single measure
	/// </summary>
	/// <typeparam name="Q">Struct type implementing the IQuantity interface</typeparam>
	public struct Measure<Q> : IMeasure<Q>, IEquatable<Measure<Q>>, IComparable<Measure<Q>> where Q : struct, IQuantity<Q>
	{
		#region MEMBER VARIABLES

		private readonly AmountType mAmount;

		#endregion

		#region CONSTRUCTORS

		/// <summary>
		/// Initializes a measure object of the specified quantity
		/// </summary>
		/// <param name="iAmount">Measured amount in reference unit, double precision</param>
		public Measure(double iAmount)
		{
			mAmount =
#if !DOUBLE
				(AmountType)
#endif
 iAmount;
		}

		/// <summary>
		/// Initializes a measure object of the specified quantity
		/// </summary>
		/// <param name="iAmount">Measured amount in reference unit, single precision</param>
		public Measure(float iAmount)
		{
			mAmount =
#if !SINGLE
 (AmountType)
#endif
iAmount;
		}

		/// <summary>
		/// Initializes a measure object of the specified quantity
		/// </summary>
		/// <param name="iAmount">Measured amount in reference unit, decimal precision</param>
		public Measure(decimal iAmount)
		{
			mAmount =
#if !DECIMAL
 (AmountType)
#endif
iAmount;
		}

		/// <summary>
		/// Initializes a measure object of a specified unit
		/// </summary>
		/// <param name="iAmount">Measured amount in double precision</param>
		/// <param name="iUnit">Unit of measure</param>
		/// <exception cref="InvalidOperationException">is thrown if the quantity of the specified unit
		/// is not the same as the type-specified quantity</exception>
		public Measure(double iAmount, IUnit<Q> iUnit)
		{
			mAmount = iUnit.AmountToReferenceUnitConverter(
#if !DOUBLE
				(AmountType)
#endif
iAmount);
		}

		/// <summary>
		/// Initializes a measure object of a specified unit
		/// </summary>
		/// <param name="iAmount">Measured amount in single precision</param>
		/// <param name="iUnit">Unit of measure</param>
		/// <exception cref="InvalidOperationException">is thrown if the quantity of the specified unit
		/// is not the same as the type-specified quantity</exception>
		public Measure(float iAmount, IUnit<Q> iUnit)
		{
			mAmount = iUnit.AmountToReferenceUnitConverter(
#if !SINGLE
(AmountType)
#endif
iAmount);
		}

		/// <summary>
		/// Initializes a measure object of a specified unit
		/// </summary>
		/// <param name="iAmount">Measured amount in decimal format</param>
		/// <param name="iUnit">Unit of measure</param>
		/// <exception cref="InvalidOperationException">is thrown if the quantity of the specified unit
		/// is not the same as the type-specified quantity</exception>
		public Measure(decimal iAmount, IUnit<Q> iUnit)
		{
			mAmount = iUnit.AmountToReferenceUnitConverter(
#if !DECIMAL
(AmountType)
#endif
iAmount);
		}

		#endregion

		#region Implementation of IMeasure<Q>

		/// <summary>
		/// Gets the measured amount in the <see cref="Unit">current unit of measure</see>
		/// </summary>
		public AmountType Amount
		{
			get { return mAmount; }
		}

		/// <summary>
		/// Gets the unit of measure
		/// </summary>
		public IUnit<Q> Unit
		{
			get { return default(Q).ReferenceUnit; }
		}

		/// <summary>
		/// Gets the amount of this measure in the requested unit
		/// </summary>
		/// <param name="iUnit">Unit to which the measured amount should be converted</param>
		/// <returns>Measured amount converted into <paramref name="iUnit">specified unit</paramref></returns>
		public AmountType GetAmount(IUnit<Q> iUnit)
		{
			return iUnit.AmountFromReferenceUnitConverter(mAmount);
		}

		#endregion

		#region PROPERTIES

		/// <summary>
		/// Gets a new unit specific measure based on this measure but in the <paramref name="iUnit">specified unit</paramref>
		/// </summary>
		/// <param name="iUnit">Unit in which the new measure should be specified</param>
		public SpecificMeasure<Q> this[IUnit<Q> iUnit]
		{
			get { return new SpecificMeasure<Q>(GetAmount(iUnit), iUnit); }
		}

		#endregion

		#region METHODS

		/// <summary>
		/// Multiply two measure objects
		/// </summary>
		/// <typeparam name="Q1">Quantity type of the left-hand side measure</typeparam>
		/// <typeparam name="Q2">Quantity type of the right-hand side measure</typeparam>
		/// <param name="iLhs">Left-hand side measure object</param>
		/// <param name="iRhs">Right-hand side measure object</param>
		/// <returns>Product of the two measure factors as a measure of the <typeparamref name="Q"/> quantity type</returns>
		public static Measure<Q> Times<Q1, Q2>(Measure<Q1> iLhs, Measure<Q2> iRhs)
			where Q1 : struct, IQuantity<Q1>
			where Q2 : struct, IQuantity<Q2>
		{
			if (default(Q).IsProductOf(default(Q1), default(Q2)))
			{
				return new Measure<Q>(iLhs.mAmount * iRhs.mAmount);
			}
			throw new InvalidOperationException(String.Format("Cannot multiply {0} and {1} to measure of quantity {2}",
															  iLhs, iRhs, default(Q).Name()));
		}

		/// <summary>
		/// Divide two measure objects
		/// </summary>
		/// <typeparam name="Q1">Quantity type of the numerator measure</typeparam>
		/// <typeparam name="Q2">Quantity type of the denominator measure</typeparam>
		/// <param name="iNumerator">Numerator measure object</param>
		/// <param name="iDenominator">Denominator measure object</param>
		/// <returns>Quotient of the two measure factors as a measure of the <typeparamref name="Q"/> quantity type</returns>
		public static Measure<Q> Divide<Q1, Q2>(Measure<Q1> iNumerator, Measure<Q2> iDenominator)
			where Q1 : struct, IQuantity<Q1>
			where Q2 : struct, IQuantity<Q2>
		{
			if (default(Q).IsQuotientOf(default(Q1), default(Q2)))
			{
				return new Measure<Q>(iNumerator.mAmount / iDenominator.mAmount);
			}
			throw new InvalidOperationException(String.Format("Cannot divide {0} and {1} to measure of quantity {2}",
															  iNumerator, iDenominator, default(Q).Name()));
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
		public bool Equals(Measure<Q> other)
		{
			return mAmount.Equals(other.mAmount);
		}

		/// <summary>
		/// Compares the current object with another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: 
		/// Value               Meaning 
		/// Less than zero      This object is less than the <paramref name="other"/> parameter.
		/// Zero                This object is equal to <paramref name="other"/>. 
		/// Greater than zero   This object is greater than <paramref name="other"/>. 
		/// </returns>
		/// <exception cref="InvalidOperationException">if the quantity of the other measure is not the same as the quantity
		/// of this object</exception>
		public int CompareTo(Measure<Q> other)
		{
			return mAmount.CompareTo(other.mAmount);
		}

		/// <summary>
		/// Compares another object with this measure object
		/// </summary>
		/// <param name="obj">Object to compare with this object</param>
		/// <returns>true if <paramref name="obj"/> is a quantity-specific Measure object that is equivalent with this object, 
		/// false otherwise</returns>
		public override bool Equals(object obj)
		{
			return obj is Measure<Q> ? Equals((Measure<Q>)obj) : false;
		}

		/// <summary>
		/// Gets the Measure object hash code
		/// </summary>
		/// <returns>Hash code of this object</returns>
		public override int GetHashCode()
		{
			return mAmount.GetHashCode();
		}

		/// <summary>
		/// Gets the measure represented as a string
		/// </summary>
		/// <returns>The measure description</returns>
		public override string ToString()
		{
			return String.Format("{0} {1}", mAmount, Unit.Symbol).Trim();
		}

		#endregion

		#region OPERATORS

		/// <summary>
		/// Converts a unit specific measure object into a generic equivalent
		/// </summary>
		/// <param name="iMeasure">Unit specific measure object</param>
		/// <returns>Generic equivalent of the unit specific measure object</returns>
		public static explicit operator Measure<Q>(SpecificMeasure<Q> iMeasure)
		{
			return new Measure<Q>(iMeasure.Unit.AmountToReferenceUnitConverter(iMeasure.Amount));
		}

		/// <summary>
		/// Double cast operator
		/// </summary>
		/// <param name="iAmount">Amount in double precision</param>
		/// <returns>Measure object with specified amount</returns>
		public static explicit operator Measure<Q>(double iAmount)
		{
			return new Measure<Q>(iAmount);
		}

		/// <summary>
		/// Float cast operator
		/// </summary>
		/// <param name="iAmount">Amount in single precision</param>
		/// <returns>Measure object with specified amount</returns>
		public static explicit operator Measure<Q>(float iAmount)
		{
			return new Measure<Q>(iAmount);
		}

		/// <summary>
		/// Decimal cast operator
		/// </summary>
		/// <param name="iAmount">Amount in decimal precision</param>
		/// <returns>Measure object with specified amount</returns>
		public static explicit operator Measure<Q>(decimal iAmount)
		{
			return new Measure<Q>(iAmount);
		}

		/// <summary>
		/// Adds two measure objects
		/// </summary>
		/// <param name="iLhs">First measure term</param>
		/// <param name="iRhs">Second measure term</param>
		/// <returns>Measure object representing the sum of the two operands</returns>
		public static Measure<Q> operator +(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return new Measure<Q>(iLhs.mAmount + iRhs.mAmount);
		}

		/// <summary>
		/// Subtract two measure objects of the same unit
		/// </summary>
		/// <param name="iLhs">First measure object</param>
		/// <param name="iRhs">Second measure object</param>
		/// <returns>Difference of the measure objects</returns>
		public static Measure<Q> operator -(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return new Measure<Q>(iLhs.mAmount - iRhs.mAmount);
		}

		/// <summary>
		/// Multiply a scalar and a measure object
		/// </summary>
		/// <param name="iScalar">Floating-point scalar</param>
		/// <param name="iMeasure">Measure object</param>
		/// <returns>Product of the scalar and the measure object</returns>
		public static Measure<Q> operator *(AmountType iScalar, Measure<Q> iMeasure)
		{
			return new Measure<Q>(iScalar * iMeasure.mAmount);
		}

		/// <summary>
		/// Multiply a measure object and a scalar
		/// </summary>
		/// <param name="iMeasure">measure object</param>
		/// <param name="iScalar">Floating-point scalar</param>
		/// <returns>Product of the measure object and the scalar</returns>
		public static Measure<Q> operator *(Measure<Q> iMeasure, AmountType iScalar)
		{
			return new Measure<Q>(iMeasure.mAmount * iScalar);
		}

		/// <summary>
		/// Divide a measure object with a scalar
		/// </summary>
		/// <param name="iMeasure">measure object</param>
		/// <param name="iScalar">Floating-point scalar</param>
		/// <returns>Quotient of the measure object and the scalar</returns>
		public static Measure<Q> operator /(Measure<Q> iMeasure, AmountType iScalar)
		{
			checked
			{
				return new Measure<Q>(iMeasure.mAmount / iScalar);
			}
		}

		/// <summary>
		/// Divide two measure objects of the same unit
		/// </summary>
		/// <param name="iNumerator">Numerator measure</param>
		/// <param name="iDenominator">Denominator measure</param>
		/// <returns>Scalar quotient of the two measure objects</returns>
		public static AmountType operator /(Measure<Q> iNumerator, Measure<Q> iDenominator)
		{
			return iNumerator.mAmount/iDenominator.mAmount;
		}

		/// <summary>
		/// Less than operator for measure objects
		/// </summary>
		/// <param name="iLhs">First object</param>
		/// <param name="iRhs">Second object</param>
		/// <returns>true if first measure object is less than second measure object; false otherwise</returns>
		public static bool operator <(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return iLhs.mAmount < iRhs.mAmount;
		}

		/// <summary>
		/// Greater than operator for measure objects
		/// </summary>
		/// <param name="iLhs">First object</param>
		/// <param name="iRhs">Second object</param>
		/// <returns>true if first measure object is greater than second measure object; false otherwise</returns>
		public static bool operator >(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return iLhs.mAmount > iRhs.mAmount;
		}

		/// <summary>
		/// Less than or equal to operator for measure objects
		/// </summary>
		/// <param name="iLhs">First object</param>
		/// <param name="iRhs">Second object</param>
		/// <returns>true if first measure object is less than or equal to second measure object; false otherwise</returns>
		public static bool operator <=(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return iLhs.mAmount <= iRhs.mAmount;
		}

		/// <summary>
		/// Greater than or equal to operator for measure objects
		/// </summary>
		/// <param name="iLhs">First object</param>
		/// <param name="iRhs">Second object</param>
		/// <returns>true if first measure object is greater than or equal to second measure object; false otherwise</returns>
		public static bool operator >=(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return iLhs.mAmount >= iRhs.mAmount;
		}

		/// <summary>
		/// Equality operator for measure objects
		/// </summary>
		/// <param name="iLhs">First object</param>
		/// <param name="iRhs">Second object</param>
		/// <returns>true if the two measure objects are equal; false otherwise</returns>
		public static bool operator ==(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return iLhs.mAmount == iRhs.mAmount;
		}

		/// <summary>
		/// Inequality operator for measure objects
		/// </summary>
		/// <param name="iLhs">First object</param>
		/// <param name="iRhs">Second object</param>
		/// <returns>true if the two measure objects are not equal; false if they are equal</returns>
		public static bool operator !=(Measure<Q> iLhs, Measure<Q> iRhs)
		{
			return iLhs.mAmount != iRhs.mAmount;
		}

		#endregion
	}
}