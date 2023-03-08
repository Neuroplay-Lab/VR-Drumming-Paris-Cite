using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class TabGroup : MonoBehaviour
	{
		#region Variables
		[SerializeField, Tooltip("List containing active tab buttons of the current group")]
		private List<TabButton> tabButtons;
		[Tooltip("Currently active tab button")]
		[SerializeField]
		private TabButton selectedTab;
		[SerializeField]
		private List<GameObject> pagesToSwitch;

		[Header("Button Colors")]
		[SerializeField] private Color tabIdleColor = Color.gray;
		[SerializeField] private Color tabHoverColor = Color.white;
		[SerializeField] private Color tabActive = Color.cyan;

		public TabGroup(List<GameObject> pagesToSwitch)
		{
			this.pagesToSwitch = pagesToSwitch;
		}

		#endregion

		/// <summary>
		/// Subscribe a passed button to the current tab group list
		/// </summary>
		/// <param name="button">TabButton reference that will be added to the list</param>
		public void Subscribe(TabButton button)
		{
			if (tabButtons == null)
			{
				tabButtons = new List<TabButton>();
			}

			tabButtons.Add(button);
		}

		public void OnTabEnter(TabButton button)
		{
			ResetTabs();
			if (selectedTab == null || button != selectedTab)
			{
				// set image color to tabHoverColor
				button.background.color = tabHoverColor;
			}
		}

		public void OnTabExit(TabButton button)
		{
			ResetTabs();
		}

		public void OnTabSelected(TabButton button)
		{
			// check if the button is already selected
			if (selectedTab != null && selectedTab == button)
			{
				selectedTab?.Deselect();
				selectedTab = null;
				ResetPages();
				ResetTabs();
				return;
			}
			selectedTab?.Deselect();
			selectedTab = button;
			selectedTab.Select();

			ResetTabs();

			button.background.color = tabActive;
			// Page Switching
			SwitchPage(button);
		}


		private void SwitchPage(TabButton button)
		{
			// Get the current button index
			var index = button.transform.GetSiblingIndex();
			// Find the corresponding index in the list of avaialble pages
			for (var i = 0; i < pagesToSwitch.Count; i++)
			{
				// If the button index matches the page index, switch the page to active
				pagesToSwitch[i].SetActive(i == index);
			}
		}
		/// <summary>
		/// Default the current tabs to their idle colour
		/// </summary>
		private void ResetTabs()
		{
			foreach(TabButton button in tabButtons)
			{
				if(selectedTab != null && button == selectedTab) {
					continue;
				}
				button.background.color = tabIdleColor;
			}
		}
		
		private void ResetPages()
		{
			foreach (var page in pagesToSwitch)
			{
				page.SetActive(false);
			}
		}
	}
}
