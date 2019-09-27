<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgressControl.ascx.cs"
    Inherits="Mediachase.Commerce.Manager.Apps.Core.Controls.ProgressControl" %>
<style>
    div.progressBarBorder
    {
        border: 1px solid;
        border-color: black;
        padding: 0px;
        text-align: left;
        height: 25px;
    }
    div.progressBar
    {
        font-family: verdana;
        font-size: 8pt;
        text-decoration: none;
        background-color: #000066;
        height: 23px;
        margin: 1px;
    }
    div.progressLog
    {
        width: 400px;
        height: 200px;
        overflow: auto;
        background-color: white;
        border: 1px solid;
        border-color: black;
        padding: 1px;
    }
    /*Modal Popup*/.modalBackground
    {
        background-color: Gray;
        filter: alpha(opacity=70);
        opacity: 0.7;
    }
    .modalHeader
    {
        border-color: #808080 #808080 #ccc;
        border-style: solid;
        border-width: 0px 1px 1px;
        padding: 0px 10px;
        color: #000000;
        font-size: 9pt;
        font-weight: bold;
        line-height: 1.9;
        font-family: arial,helvetica,clean,sans-serif;
    }
    .modalPopup
    {
        background-color: #f2f2f2;
        background-color: #f2f2f2;
        border-color: #808080;
        border-style: solid;
        border-width: 3px;
        padding: 3px;
    }
</style>
<script type="text/javascript">
    function ToggleLog() {
        var log = document.getElementById('ProgressLog');
        
        if(log.style.display == 'none')
            log.style.display = '';
        else
            log.style.display = 'none';
    }
</script>
<asp:Panel ID="Panel1" runat="server" Style="display: none" CssClass="modalPopup">
    <asp:Panel ID="Panel3" runat="server" CssClass="modalHeader" Style="cursor: move;
        background-color: #DDDDDD; border: solid 1px Gray; color: Black">
        <div>
            <p><asp:Label runat="server" ID="TitleLabel" Text="<%$ Resources:CoreStrings, Progress_Indicator %>"></asp:Label></p>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="UpdatePanelWithTimer" runat="server" ChildrenAsTriggers="true"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField runat="server" ID="LogStatus" Value="" />
            <asp:Timer runat="server" ID="pnlTimer" Enabled="false" OnTick="OnTimer" Interval="3000">
            </asp:Timer>
            <div class="w100">
                <!-- START: ProgressBar -->
                <div style="text-align: center;">
                    <span runat="server" id="amountComplete" style="font-family: Verdana, Arial;"></span>
                    <br />
                    <div class="progressBarBorder" style="width: 400px">
                        <div runat="server" id="statusBar" class="progressBar" visible="false" style="width: 0px;">
                        </div>
                    </div>
                </div>
                <!-- END: ProgressBar -->
                <!-- START: Progress status description -->
                <div style="text-align: center; padding-left: 5px; width: 390px; overflow: hidden;">
                    <asp:Label runat="server" ID="ProgressStatusMessage"></asp:Label>
                </div>
                <br />
                <!-- END: Progress status description -->
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:CoreStrings, Progress_Operation_Log %>"/>:<br />
                <div class="progressLog" id="lblProgress" runat="server"></div>
                <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:LinkButton runat="server" ID="CancelButton" Text="<%$ Resources:SharedStrings, Close_Window %>"></asp:LinkButton>
</asp:Panel>
<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none" />
<ajaxToolkit:ModalPopupExtender runat="server" ID="ModalPopupExtender" BehaviorID="programmaticModalPopupBehavior"
    TargetControlID="hiddenTargetControlForModalPopup" CancelControlID="CancelButton"
    PopupControlID="Panel1" BackgroundCssClass="modalBackground" DropShadow="True"
    PopupDragHandleControlID="Panel3" RepositionMode="RepositionOnWindowScroll">
</ajaxToolkit:ModalPopupExtender>
