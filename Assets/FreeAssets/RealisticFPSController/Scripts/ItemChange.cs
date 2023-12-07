using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XEntity.InventoryItemSystem;

namespace EvolveGames
{

        public class ItemChange : MonoBehaviour
        {
            [Header("Item Change")]
            public Animator ani;
            public Image ItemCanvasLogo;
            public bool LoopItems = true;
            public List<GameObject> Items = new List<GameObject>();  // Ensure that the list is initialized
            public  List<Sprite> ItemLogos;
            public int ItemIdInt;
            public int MaxItems;
            int ChangeItemInt;
            [HideInInspector] public bool DefiniteHide;
            bool ItemChangeLogo;

            
            // Item objects
            public GameObject flashlight;
            public GameObject crowbar;
            public GameObject axe;
            public GameObject katana;
            public GameObject hammer;

            //Item bools
            private bool axeBool;
            private bool crowbarBool;
            private bool katanaBool;
            private bool hammerBool;

            //Item renderers
            SpriteRenderer spriteRenderer;
            SpriteRenderer axeRenderer;
            SpriteRenderer katanaRenderer;
            SpriteRenderer crowbarRenderer;
            SpriteRenderer hammerRenderer;

            //Item sprites
            Sprite axeSprite;
            Sprite crowbarSprite;
            Sprite katanaSprite;
            Sprite hammerSprite;

            ItemContainer playerInventory;
            private void Awake()
            {
                Items = new List<GameObject>();
            }

            private void Start()
            {
                GameObject FlashLight1 = GameObject.Find("FlashlightSprite");
                spriteRenderer = FlashLight1.GetComponent<SpriteRenderer>();
                Sprite flashlightImg = spriteRenderer.sprite;
                AddItemToList(flashlight);
                AddImage(flashlightImg);
                if (ani == null && GetComponent<Animator>()) ani = GetComponent<Animator>();
                Color OpacityColor = ItemCanvasLogo.color;
                OpacityColor.a = 0;
                ItemCanvasLogo.color = OpacityColor;
                ItemChangeLogo = false;
                DefiniteHide = false;
                ChangeItemInt = ItemIdInt;
                ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];

                GameObject playerObject = GameObject.FindWithTag("PlayerInv");
                playerInventory = playerObject.GetComponent<ItemContainer>();

            // Initialisation of the items that are available to be picked up into the player's hand \__-_-__/
            GameObject axe1 = GameObject.Find("Axe");
            axeRenderer = axe1.GetComponent<SpriteRenderer>();
            axeSprite = axeRenderer.sprite;

            GameObject crowbar1 = GameObject.Find("CrowbarSprite");
            crowbarRenderer = crowbar1.GetComponent<SpriteRenderer>();
            crowbarSprite = crowbarRenderer.sprite;

            GameObject katana1 = GameObject.Find("KatanaSprite");
            katanaRenderer = katana1.GetComponent<SpriteRenderer>();
            katanaSprite = katanaRenderer.sprite;

            GameObject hammer1 = GameObject.Find("HammerSprite");
            hammerRenderer = hammer1.GetComponent<SpriteRenderer>();
            hammerSprite = hammerRenderer.sprite;



            StartCoroutine(ItemChangeObject());
            }
            private void Update()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                ItemIdInt++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                ItemIdInt--;
            }

            if(Input.GetKeyDown(KeyCode.H))
            {
                if (ani.GetBool("Hide")) Hide(false);
                else Hide(true);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                // Add the current GameObject (this script's GameObject) to the list
                AddItemToList(crowbar);
            }

            if (ItemIdInt < 0) ItemIdInt = LoopItems ? MaxItems : 0;
            if (ItemIdInt > MaxItems) ItemIdInt = LoopItems ? 0 : MaxItems;


            if (ItemIdInt != ChangeItemInt)
            {
                ChangeItemInt = ItemIdInt;
                StartCoroutine(ItemChangeObject());
            }


            if (playerInventory.ContainsItemName("Axe") && !axeBool)
            {
                axeBool = true;
                AddItemToList(axe);
                AddImage(axeSprite);
            }
            if(playerInventory.ContainsItemName("Crowbar") && !crowbarBool)
            {
                crowbarBool = true;
                AddItemToList(crowbar);
                AddImage(crowbarSprite);
            }
            if (playerInventory.ContainsItemName("Katana") && !katanaBool)
            {
                katanaBool = true;
                AddItemToList(katana);
                AddImage(katanaSprite);
            }
            if (playerInventory.ContainsItemName("Hammer") && !hammerBool)
            {
                hammerBool = true;
                AddItemToList(hammer);
                AddImage(hammerSprite);
            }
        }

        public void Hide(bool Hide)
        {
            DefiniteHide = Hide;
            ani.SetBool("Hide", Hide);
        }

        IEnumerator ItemChangeObject()
        {
            if(!DefiniteHide) ani.SetBool("Hide", true);
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < (MaxItems + 1); i++)
            {
                Items[i].SetActive(false);
            }
            Items[ItemIdInt].SetActive(true);
            if (!ItemChangeLogo) StartCoroutine(ItemLogoChange());

            if (!DefiniteHide) ani.SetBool("Hide", false);
        }

        IEnumerator ItemLogoChange()
        {
            ItemChangeLogo = true;
            yield return new WaitForSeconds(0.5f);
            ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];
            yield return new WaitForSeconds(0.1f);
            ItemChangeLogo = false;
        }

        private void FixedUpdate()
        {
            
            if (ItemChangeLogo)
            {
                Color OpacityColor = ItemCanvasLogo.color;
                OpacityColor.a = Mathf.Lerp(OpacityColor.a, 0, 20 * Time.deltaTime);
                ItemCanvasLogo.color = OpacityColor;
            }
            else
            {
                Color OpacityColor = ItemCanvasLogo.color;
                OpacityColor.a = Mathf.Lerp(OpacityColor.a, 1, 6 * Time.deltaTime);
                ItemCanvasLogo.color = OpacityColor;
            }
        }
        public void AddItemToList(GameObject newItem)
        {
            if (newItem != null)
            {
                Items.Add(newItem);
                MaxItems = Items.Count - 1;
            }
            else
            {
                Debug.LogError("Attempted to add a null item to the list.");
            }
        }
        public void AddImage(Sprite a)
        {
            ItemLogos.Add(a);
        }

    }

}
