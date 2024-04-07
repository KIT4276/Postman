using System;
using System.Collections.Generic;

public class Post
{
    private string _postAddressID;

    private readonly List<string> _addressesId = new();

    private readonly Dictionary<string, AddressTrigger> _addressesDic = new();

    public event Action PostEnterE;
    public event Action AddressEnterE;

    public void SetAddress(AddressTrigger address)
    {
        if (address.GetComponent<PostAddressTrigger>() != null)
            _postAddressID = address.Id;
        else
        {
            _addressesId.Add(address.Id);

        }
        _addressesDic.Add(address.Id, address);

        address.AddressTriggerEnter += OnAddressEnter;
    }

    private void OnAddressEnter(string id)
    {
        if (id == _postAddressID)
            PostEnterE?.Invoke();

        else
            AddressEnterE?.Invoke();
    }

    public List<string> GetAllAddressesID() =>
        _addressesId;

    public AddressTrigger GetAddressTrigger(string id) =>
        _addressesDic[id];

    public void TurnOnPostTrigger() => 
        _addressesDic[_postAddressID].gameObject.SetActive(true);
}
