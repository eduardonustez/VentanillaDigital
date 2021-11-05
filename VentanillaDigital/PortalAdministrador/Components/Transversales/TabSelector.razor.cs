using GenericExtensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalAdministrador.Components.Transversales
{
    public partial class TabSelector 
    {
        [Parameter]
        public String Current { 
            get => ActiveTab?.Value.Name;
            set
            {
                var node = TabList.FindFirst(x => x.Name == value);

                if (node == null)
                    return;

                ActiveTab = node;
            } 
        }

        [Parameter]
        public EventCallback<String> CurrentChanged { get; set; }

        public bool NextDisabled
        {
            get => (Next?.Value == null || Next.Value.Disabled);
        }

        [Parameter]
        public EventCallback<bool> NextDisabledChanged { get; set; }

        public bool PreviousDisabled
        {
            get => (Previous?.Value == null || Previous.Value.Disabled);
        }

        [Parameter]
        public EventCallback<bool> PreviousDisabledChanged { get; set; }

        private LinkedListNode<Tab> Next 
        {
            get => ActiveTab?.FindNext(x => x.Display,false);
        }

        private LinkedListNode<Tab> Previous
        {
            get => ActiveTab?.FindPrevious(x => x.Display,false);
        }

        public async Task UpdateNextAndPreviousDisabled ()
        {
            Console.WriteLine("Updating");
            await PreviousDisabledChanged.InvokeAsync(PreviousDisabled);
            await NextDisabledChanged.InvokeAsync(NextDisabled);
        }

        private LinkedListNode<Tab> _activeTab;
        private LinkedListNode<Tab> ActiveTab { 
            get => _activeTab; 
            set { 
                _activeTab = value; 
                StateHasChanged(); 
            } 
        }

        private LinkedList<Tab> TabList = new LinkedList<Tab>();

        public async Task Select (string name)
        {
            Current = name;
            await CurrentChanged.InvokeAsync(Current);
            await UpdateNextAndPreviousDisabled();
        }

        public async Task SelectNext ()
        {
            if (NextDisabled)
                return;
            try
            {
                await ActiveTab.Value.OnNext.InvokeAsync(null);

                ActiveTab = Next;
                await CurrentChanged.InvokeAsync(Current);
                await UpdateNextAndPreviousDisabled();
            }
            catch 
            {

            }
        }

        public async Task SelectPrevious()
        {
            if (PreviousDisabled)
                return;

            ActiveTab = Previous;
            await CurrentChanged.InvokeAsync(Current);
            await UpdateNextAndPreviousDisabled();
        }

        internal async Task AddTab (Tab tab)
        {
            Console.WriteLine($"AddTab {tab.Name}");
            TabList.AddLast(tab);
            if (TabList.Count == 1)
            {
                ActiveTab = TabList.First;
                await CurrentChanged.InvokeAsync(Current);
            }
            await UpdateNextAndPreviousDisabled();
        }

        internal async Task RemoveTab(Tab tab)
        {
            if (ActiveTab?.Value == tab)
            {
                if (ActiveTab != TabList.First)
                    await SelectPrevious();
                else if (ActiveTab != TabList.Last)
                    await SelectNext();
                else
                {
                    ActiveTab = null;
                    await CurrentChanged.InvokeAsync(Current);
                }
            }
            TabList.Remove(tab);
            await UpdateNextAndPreviousDisabled();
        }
    }
}
