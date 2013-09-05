$AnalysisConsole = "Analyzer.Console\bin\Debug\Analyzer.Console.exe"
$Svn = "C:\Program Files\SlikSvn\bin\svn.exe"

$RootDacpacPath = "\\hq.echogl.net\files\Development\Release\Dacpac\Prod"
$RootAssemblyPath = "\\hq.echogl.net\files\Development\Release\Assemblies"
$RootAssembliesThirdPartyPath = "\\hq.echogl.net\files\Development\Release\Assemblies3rdParty"
$RootPackagePath = "\\hq.echogl.net\files\Development\Release\Packages\CDS"
$SubversionRootUrl = "svn://svn.echo.com:4345"
$SubversionUsername = "build"
$SubversionPassword = "saucecontrol"

$DB01Dacpacs = 
    "API_DB.PROD.dacpac",
    "CarrierCapacity.PROD.dacpac",
    "EchoLogin2.PROD.dacpac",
    "EchoODS.PROD.dacpac",
    "EchoOptimizer.PROD.dacpac",
    "EchoOptimizerImages.PROD.dacpac",
    "EchoQuote.PROD.dacpac",
    "EchoTrak.PROD.dacpac",
    "EDIStaging.PROD.dacpac",
    "FreightClassLookup.PROD.dacpac",
    "GeoCache.PROD.dacpac",
    "LoadLink.PROD.dacpac",
    "LoadPosting.PROD.dacpac",
    "LockingManager.PROD.dacpac",
    "Mdm.PROD.dacpac",
    "PurchaseOrder.PROD.dacpac",
    "RateIQ.PROD.dacpac",
    "RateIQ2.PROD.dacpac",
    "SearchRepository.PROD.dacpac",
    "Truckload.PROD.dacpac"

$DB02Dacpacs =
    "EchoOptimizer.DB02.PROD.dacpac"

$AssembliesThirdParty =
	"CFF\clarityCore.dll",
	"Echo\Echo.Common.UserComponent.dll",
	"log4net\log4net.dll",
	"MicrosoftUnity\V2\Microsoft.Practices.ServiceLocation.dll",
	"MicrosoftUnity\V2\Microsoft.Practices.Unity.dll",
	"MicrosoftUnity\V2\Microsoft.Practices.Unity.Configuration.dll",
	"MicrosoftReportViewer\Microsoft.ReportViewer.WebForms.dll",
	"PeterBlum\PeterBlum.ADME.dll",
	"PeterBlum\PeterBlum.VAM.dll",
	"PeterBlum\PeterBlum.VAM.MSStyle.dll",
	"PowerXML\PowerXML.dll"
	
$Assemblies =
	"Echo.BDP.Business\Implementations\Trunk\Echo.BDP.Business.Data.dll",
	"Echo.BDP.Business\Implementations\Trunk\Echo.BDP.Business.Data.SqlClient.dll",
	"Echo.BDP.Business\Implementations\Trunk\Echo.BDP.Business.Domain.dll",
	"Echo.BDP.Business\Implementations\Trunk\Echo.BDP.Business.Entities.dll",
	"Echo.BDP.Business\Implementations\Trunk\Echo.BDP.Business.Web.dll",
	"Echo.BE.Shipment\Trunk\Echo.BE.Shipment.dll",
	"Echo.BSS.LTLAutoTender.InterfaceDatagrams\Interfaces\1.0.0.0\Echo.BSS.LTLAutoTender.InterfaceDatagrams.dll",
	"Echo.BSS.LoadPosting.Service\Trunk\Echo.BSS.LoadPosting.Domain.dll",
	"Echo.BSS.LoadPosting.Service\Trunk\Echo.BSS.LoadPosting.Service.Proxy.dll",
	"Echo.BSS.OptimizerLTLAutoTenderService\Trunk\Echo.BSS.OptimizerLTLAutoTender.dll",
	"Echo.BSS.OptimizerLTLAutoTenderService.Interfaces\Interfaces\1.0.0.0\Echo.BSS.OptimizerLTLAutoTender.Interface.dll",
	"Echo.BSS.PurchaseInsurance\Implementations\Trunk\Echo.BSS.PurchaseInsurance.Proxy.dll",
	"Echo.BSS.PurchaseInsurance\Implementations\Trunk\Echo.BSS.PurchaseInsurance.Services.Contracts.dll",
	"Echo.BSS.PurchaseOrder.Services\Trunk\Echo.BSS.PurchaseOrder.Services.Contracts.dll",
	"Echo.BSS.PurchaseOrder.Services\Trunk\Echo.BSS.PurchaseOrder.Services.WcfProxy.dll",
	"Echo.BSS.ShipmentCarrierCapacity\Trunk\Echo.BSS.ShipmentCarrierCapacity.Proxy.dll",
	"Echo.BSS.ShipmentCarrierCapacity\Trunk\Echo.BSS.ShipmentCarrierCapacity.Services.Contracts.dll",
	"Echo.BSS.ShipmentLocationTracking\Trunk\Echo.BSS.ShipmentLocationTracking.Service.Contracts.dll",
	"Echo.Common.Business\Implementations\Trunk\Echo.Common.Business.dll",
	"Echo.Common.Data\Implementations\Trunk\Echo.Common.Data.dll",
	"Echo.Common.Security\Implementations\Trunk\Echo.Common.Security.dll",
	"Echo.Enterprise.EventCloud.Common\Implementations\Trunk\Echo.Enterprise.EventCloud.Common.dll",
	"Echo.Enterprise.Framework\Implementations\Trunk\Echo.Enterprise.Framework.dll",
	"Echo.MDS.Equipment\Trunk\Echo.MDS.Equipment.Proxy.dll",
	"Echo.Optimizer\OptimizerMappoint\2.0.0.0\OptimizerMappoint.dll",
    "Echo.Optimizer\OptimizerBusinessLayer\Trunk\OptimizerBusinessLayer.dll",
	"Echo.UIC.Equipment\Trunk\Echo.UIC.Equipment.WcfService.Contracts.dll",
	"Echo.UIC.Equipment\Trunk\Echo.UIC.Equipment.WcfService.Proxy.dll",
	"Echo.UIP.Equipment\RB-1.1.0\Echo.UIP.Equipment.PE.dll",
	"Echo.USS.LocationTracking\Trunk\Echo.USS.LocationTracking.Services.Contracts.dll",
	"Echo.USS.AddressValidation\Helpers\Trunk\Echo.USS.AddressValidation.Helpers.dll",
	"Echo.USS.AddressValidation\Interface\Trunk\Echo.USS.AddressValidation.Contracts.dll",
	"Echo.USS.Search\Implementations\Echo.Search.Geo\Trunk\Echo.Search.Geo.dll",
	"Echo.USS.Search\Implementations\Echo.Search.Organization\2.1.2\Echo.Search.Organization.dll",
	"EGL_Optimizer\Implementations\2.0.0.0\EGL_Optimizer.Domain.dll"
	
$PackagedAssemblies =
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.BusinessLayer.dll",
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.EngineExample.DomainModel.dll",
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.EngineExample.Factory.dll",
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.Loader.dll",
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.Poco.dll",
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.RateServiceFacade.dll",
	"Echo.BSS.RateIQ2\Trunk\WEB\EchoRateIQ.Trunk.*.zip:bin/Echo.RateIQ.Repository.dll",
	"Echo.Optimizer.OptimizerShipKitEDIService\Trunk\Console\OptimizerShipKitEDIService.Trunk.*.zip:bin\Release\OptimizerShipKitEDIService.exe",
	"Echo.Optimizer.OptimizerShipKitEDIService\Trunk\Console\OptimizerShipKitEDIService.Trunk.*.zip:bin\Release\optIntShipKitEDI.dll",
	"Echo.Optimizer.WebUI\Trunk\WEB\Echo.Optimizer.WebUI.*.zip:bin/Echo.Optimizer.WebUI.dll"

$DotNetSubversionPaths =
	"Echo.Common.Data/Trunk/src",
	"Echo.Common.Business/Trunk/src",
	"Echo.Common.Security/Trunk/src",
	"Echo.Optimizer.BusinessLayer/Trunk",
	"Echo.Optimizer.ShipKitEDI/OptimizerShipKitEDIService/Trunk",
	"Echo.Optimizer.ShipKitEDI/optIntShipKitEDI/Trunk",
	"Echo.Optimizer.WebUI/Trunk"
	
$ReportPaths =
	"C:\Echo Development\Analyzer Artifacts\RDL"


$DB01Dacpacs | % {
	"Analyzing $_ for node discovery"
    &$AnalysisConsole -dacpac "$RootDacpacPath\$_" -server "DB01VPRD"
}

$DB01Dacpacs | % {
	"Analyzing $_ for reference relationships"
    &$AnalysisConsole -dacpac-references "$RootDacpacPath\$_" -server "DB01VPRD"
}

$DB02Dacpacs | % {
	"Analyzing $_ for node discovery"
    &$AnalysisConsole -dacpac "$RootDacpacPath\$_" -server "DB02VPRD"
}

$DB02Dacpacs | % {
	"Analyzing $_ for reference relationships"
    &$AnalysisConsole -dacpac-references "$RootDacpacPath\$_" -server "DB02VPRD"
}
