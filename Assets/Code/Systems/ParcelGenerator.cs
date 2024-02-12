using System;
using System.Collections.Generic;

public class ParcelGenerator
{
    public string CurrentAddress { get; private set; }

    private readonly Post _post;

    public event Action GenerateAddressE;

    public ParcelGenerator(Post post)
    {
        _post = post;

        _post.PostEnterE += GenerateAddress;
        _post.AddressEnterE += PostTriggerOn;
    }

    public void StartGenerate() => 
        OffAllAdrressesObj();

    private void PostTriggerOn()
    {
        _post.TurnOnPostTrigger();
        GenerateAddressE?.Invoke();
    }

    private void GenerateAddress() =>
        _post.GetAddressTrigger(RandomAddress()).gameObject.SetActive(true);

    private void OffAllAdrressesObj()
    {
        foreach (var address in _post.GetAllAddressesID())
        {
            _post.GetAddressTrigger(address).gameObject.SetActive(false);
        }
    }

    private string RandomAddress()
    {
        List<string> addresses = _post.GetAllAddressesID();
        int r = UnityEngine.Random.Range(0, addresses.Count);
        CurrentAddress = addresses[r];
        return addresses[r];
    }
}
