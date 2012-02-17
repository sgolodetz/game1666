/***
 * game1666proto4: PriorityQueue.cs
 * Copyright 2012. All rights reserved.
 ***/

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace game1666proto4.Common.ADTs
{
	/// <summary>
	/// This is an implementation of priority queues that allows the keys of queue elements to be updated in-place.
	/// To do this, it maintains a dictionary (that allows elements to be looked up) alongside the usual heap-based
	/// priority queue implementation.
	/// </summary>
	/// <typeparam name="ID">The element ID type (used for the lookup).</typeparam>
	/// <typeparam name="Key">The key type (the type of the priority values used to determine the element order).</typeparam>
	/// <typeparam name="Data">The auxiliary data type (any information clients might wish to store with each element).</typeparam>
	sealed class PriorityQueue<ID, Key, Data> where ID : IEquatable<ID>
	{
		//#################### NESTED CLASSES ####################
		#region

		/// <summary>
		/// Each element of the priority queue stores its ID, its key and potentially some auxiliary data
		/// that may be useful to client code. Its auxiliary data may be changed by the client, but its
		/// key should only be changed via the priority queue's UpdateKey() method (this is important!).
		/// </summary>
		public class Element
		{
			public ID ID { get; private set; }
			public Key Key { get; set; }
			public Data Data { get; set; }

			public Element(ID id, Key key, Data data)
			{
				ID = id;
				Key = key;
				Data = data;
			}
		}

		#endregion

		//#################### PRIVATE VARIABLES ####################
		#region

		// Datatype Invariant: m_dictionary.Count == m_heap.Count

		/// <summary>
		/// The dictionary used to look up elements by ID in the priority queue.
		/// </summary>
		private Dictionary<ID,int> m_dictionary = new Dictionary<ID,int>();

		/// <summary>
		/// The heap used to manage the ordering of elements in the priority queue.
		/// </summary>
		private List<Element> m_heap = new List<Element>();

		/// <summary>
		/// The comparer used to order the elements by key in the heap.
		/// </summary>
		private readonly IComparer<Key> m_keyComparer;

		#endregion

		//#################### PROPERTIES ####################
		#region

		/// <summary>
		/// The number of elements in the priority queue.
		/// </summary>
		public int Count { get { return m_dictionary.Count; } }

		/// <summary>
		/// Whether or not the priority queue is empty.
		/// </summary>
		public bool Empty { get { return m_dictionary.Count == 0; } }

		/// <summary>
		/// The element at the front of the priority queue.
		/// </summary>
		public Element Top
		{
			get
			{
				Contract.Requires(!Empty);
				return m_heap[0];
			}
		}

		#endregion

		//#################### CONSTRUCTORS ####################
		#region

		/// <summary>
		/// Constructs a new priority queue that uses the specified key comparer.
		/// </summary>
		/// <param name="keyComparer">The key comparer.</param>
		public PriorityQueue(IComparer<Key> keyComparer)
		{
			m_keyComparer = keyComparer;
		}

		#endregion

		//#################### PUBLIC METHODS ####################
		#region

		/// <summary>
		/// Clears the priority queue.
		/// </summary>
		public void Clear()
		{
			m_dictionary.Clear();
			m_heap.Clear();

			EnsureInvariant();
		}

		/// <summary>
		/// Returns whether or not the priority queue contains an element with the specified ID.
		/// </summary>
		/// <param name="id">The ID.</param>
		/// <returns>true, if it does contain such an element, or false otherwise.</returns>
		public bool Contains(ID id)
		{
			return m_dictionary.ContainsKey(id);
		}

		/// <summary>
		/// Erases the element with the specified ID from the priority queue.
		/// </summary>
		/// <param name="id">The ID.</param>
		public void Erase(ID id)
		{
			Contract.Requires(Contains(id));
			Contract.Ensures(!Contains(id));

			int i = m_dictionary[id];
			m_dictionary.Remove(id);
			m_heap[i] = m_heap[m_heap.Count - 1];

			// Assuming the element we were erasing wasn't the last one in the heap, update the dictionary.
			if(!m_heap[i].ID.Equals(id))
			{
				m_dictionary[m_heap[i].ID] = i;
			}

			m_heap.RemoveAt(m_heap.Count - 1);
			Heapify(i);

			EnsureInvariant();
		}

		/// <summary>
		/// Gets the element with the specified ID.
		/// </summary>
		/// <param name="id">The ID.</param>
		/// <returns>The element.</returns>
		public Element GetElement(ID id)
		{
			Contract.Requires(Contains(id));
			return m_heap[m_dictionary[id]];
		}

		/// <summary>
		/// Inserts a new element into the priority queue.
		/// </summary>
		/// <param name="id">The new element's ID.</param>
		/// <param name="key">The new element's key.</param>
		/// <param name="data">The new element's auxiliary data.</param>
		public void Insert(ID id, Key key, Data data)
		{
			if(Contains(id))
			{
				throw new InvalidOperationException("An element with the specified ID is already in the priority queue.");
			}

			int i = m_heap.Count;
			m_heap.Add(null);
			while(i > 0 && m_keyComparer.Compare(key, m_heap[Parent(i)].Key) < 0)
			{
				int p = Parent(i);
				m_heap[i] = m_heap[p];
				m_dictionary[m_heap[i].ID] = i;
				i = p;
			}
			m_heap[i] = new Element(id, key, data);
			m_dictionary[id] = i;

			EnsureInvariant();
		}

		/// <summary>
		/// Removes the element at the front of the priority queue.
		/// </summary>
		public void Pop()
		{
			Contract.Requires(!Empty);

			Erase(m_heap[0].ID);
			EnsureInvariant();
		}

		/// <summary>
		/// Updates the key of the specified element with a new value. This potentially
		/// involves an internal reordering of the priority queue's heap.
		/// </summary>
		/// <param name="id">The ID of the element whose key is to be updated.</param>
		/// <param name="key">The new key value.</param>
		public void UpdateKey(ID id, Key key)
		{
			Contract.Requires(Contains(id));

			int i = m_dictionary[id];
			UpdateKeyAt(i, key);

			EnsureInvariant();
		}

		#endregion

		//#################### PRIVATE METHODS ####################
		#region

		/// <summary>
		/// Checks the datatype invariant and throws an exception if it has been violated.
		/// </summary>
		private void EnsureInvariant()
		{
			if(m_dictionary.Count != m_heap.Count)
			{
				throw new InvalidOperationException("The operation which just executed invalidated the priority queue.");
			}
		}

		/// <summary>
		/// Floats the specified element downwards away from the root of the heap
		/// as necessary so as to restore the heap invariant. This is necessary
		/// when an element's key changes in such a way as to decrease the element's
		/// priority.
		/// </summary>
		/// <param name="i">The heap index of the element that may need to be floated downwards.</param>
		private void Heapify(int i)
		{
			bool done = false;
			while(!done)
			{
				int left = Left(i), right = Right(i);
				int largest = i;

				if(left < m_heap.Count && m_keyComparer.Compare(m_heap[left].Key, m_heap[largest].Key) < 0)
				{
					largest = left;
				}

				if(right < m_heap.Count && m_keyComparer.Compare(m_heap[right].Key, m_heap[largest].Key) < 0)
				{
					largest = right;
				}

				if(largest != i)
				{
					// Swap m_heap[i] and m_heap[largest].
					Element temp = m_heap[i];
					m_heap[i] = m_heap[largest];
					m_heap[largest] = temp;

					m_dictionary[m_heap[i].ID] = i;
					m_dictionary[m_heap[largest].ID] = largest;
					i = largest;
				}
				else done = true;
			}
		}

		/// <summary>
		/// Determines the index of element i's left child in the heap.
		/// </summary>
		/// <param name="i">The index of the heap element whose left child we want to determine.</param>
		/// <returns>The index of element i's left child.</returns>
		private static int Left(int i)
		{
			return 2*i + 1;
		}

		/// <summary>
		/// Determines the index of element i's parent in the heap.
		/// </summary>
		/// <param name="i">The index of the heap element whose parent we want to determine.</param>
		/// <returns>The index of element i's parent.</returns>
		private static int Parent(int i)
		{
			return (i+1)/2 - 1;
		}

		/// <summary>
		/// Percolates the specified element upwards towards the root of the heap
		/// as necessary so as to restore the heap invariant. This is necessary
		/// when an element's key changes in such a way as to increase the element's
		/// priority.
		/// </summary>
		/// <param name="i">The heap index of the element that may need to be percolated.</param>
		private void Percolate(int i)
		{
			while(i > 0 && m_keyComparer.Compare(m_heap[i].Key, m_heap[Parent(i)].Key) < 0)
			{
				int p = Parent(i);

				// Swap m_heap[i] and m_heap[p].
				Element temp = m_heap[i];
				m_heap[i] = m_heap[p];
				m_heap[p] = temp;

				m_dictionary[m_heap[i].ID] = i;
				m_dictionary[m_heap[p].ID] = p;
				i = p;
			}
		}

		/// <summary>
		/// Determines the index of element i's right child in the heap.
		/// </summary>
		/// <param name="i">The index of the heap element whose right child we want to determine.</param>
		/// <returns>The index of element i's right child.</returns>
		private static int Right(int i)
		{
			return 2*i + 2;
		}

		/// <summary>
		/// Updates the key of the specified element with a new value. This potentially
		/// involves an internal reordering of the priority queue's heap.
		/// </summary>
		/// <param name="i">The heap index of the element whose key is to be updated.</param>
		/// <param name="key">The new key value.</param>
		private void UpdateKeyAt(int i, Key key)
		{
			int comparison = m_keyComparer.Compare(key, m_heap[i].Key);

			if(comparison < 0)
			{
				// The element's priority has increased.
				m_heap[i].Key = key;
				Percolate(i);
			}
			else if(comparison > 0)
			{
				// The element's priority has decreased.
				m_heap[i].Key = key;
				Heapify(i);
			}
		}

		#endregion
	}
}
