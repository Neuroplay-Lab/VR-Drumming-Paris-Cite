using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
	/// <summary>
	/// A tab button that can be selected and deselected, only one tab can be open at a time.
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
	{
		#region Attributes
		[SerializeField]
		private TabGroup tabGroup;
		[SerializeField]
		private UnityEvent tabSelected;
		[SerializeField]
		private UnityEvent tabDeselected;

		public Image background;
	
		#endregion

		#region Unity Methods

		private void Awake()
		{
			background = GetComponent<Image>();
		}
		

		private void Start()
		{
			tabGroup.Subscribe(this);
		}

		/// <summary>
		/// Callback for tab button being selected
		/// </summary>
		public void Select()
		{
			tabSelected?.Invoke();
		}

		/// <summary>
		/// Callback for deselection of a button
		/// </summary>
		public void Deselect()
		{
			tabDeselected?.Invoke();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			tabGroup.OnTabSelected(this);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			tabGroup.OnTabEnter(this);

		}

		public void OnPointerExit(PointerEventData eventData)
		{
			tabGroup.OnTabExit(this);

		}
		#endregion
	}
}
