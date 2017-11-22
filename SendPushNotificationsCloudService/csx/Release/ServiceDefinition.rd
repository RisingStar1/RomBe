<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="SendPushNotificationsCloudService" generation="1" functional="0" release="0" Id="a1f2ea61-ab99-4295-86c4-d07e37bfd056" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="SendPushNotificationsCloudServiceGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="SendPushNotificationsWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/SendPushNotificationsCloudService/SendPushNotificationsCloudServiceGroup/MapSendPushNotificationsWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapSendPushNotificationsWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/SendPushNotificationsCloudService/SendPushNotificationsCloudServiceGroup/SendPushNotificationsWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="SendPushNotificationsWorkerRole" generation="1" functional="0" release="0" software="C:\Users\Almog\Documents\Visual Studio 2013\Projects\RomBe\SendPushNotificationsCloudService\csx\Release\roles\SendPushNotificationsWorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;SendPushNotificationsWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;SendPushNotificationsWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/SendPushNotificationsCloudService/SendPushNotificationsCloudServiceGroup/SendPushNotificationsWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/SendPushNotificationsCloudService/SendPushNotificationsCloudServiceGroup/SendPushNotificationsWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/SendPushNotificationsCloudService/SendPushNotificationsCloudServiceGroup/SendPushNotificationsWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="SendPushNotificationsWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="SendPushNotificationsWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="SendPushNotificationsWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>