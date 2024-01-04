<%@ Page Title="Monitor Cartoni" Language="C#" MasterPageFile="~/Root.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PackageMonitoringXCM.FormLayout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" type="text/css" href='<%# ResolveUrl("~/Content/FormLayout.css") %>' />
    <%--TODO: Funzione JS per disabilitare la tastiera, su android non dovrebbe mostrarla a schermo, DA PROVARE. --%>
    <script type="text/javascript">
        //function disable() {
        //    document.onkeydown = function (e) {
        //        return false;
        //    }
        //}
        function checkGridView() {
            var rowCount = <%= GridView1.VisibleRowCount%>
            var idDocumento = <%= idDocumentoTextBox.Text %>
            console.log(-1);

            if (rowCount == 0 && idDocumento != null) {
                alert("Attenzione: la tabella è vuota");
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="PageContent" runat="server">
    <div class="formLayoutContainer">
        <dx:ASPxFormLayout runat="server" ID="ASPxFormLayout" Width="100%" ClientInstanceName="FormLayout"
            UseDefaultPaddings="false" CssClass="formLayout" RequiredMarkDisplayMode="RequiredOnly">
            <Items>
                <dx:LayoutGroup Width="100%" Caption="Monitor Contenitori XCM" ColumnCount="2">
                    <GridSettings StretchLastItem="true" WrapCaptionAtWidth="660">
                        <Breakpoints>
                            <dx:LayoutBreakpoint MaxWidth="500" ColumnCount="1" Name="S" />
                            <dx:LayoutBreakpoint MaxWidth="800" ColumnCount="2" Name="M" />
                        </Breakpoints>
                    </GridSettings>
                    <Items>
                        <dx:LayoutItem Caption="ID Documento" VerticalAlign="Middle">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="idDocumentoTextBox" runat="server" Width="100%" OnTextChanged="customTextChanged" AutoPostBack="true" AutoCompleteType="Disabled">
                                        
                                        <MaskSettings Mask="<####################>" AllowMouseWheel="False" IncludeLiterals="None"  PromptChar=" "/>
                                        
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="Text">
                                            <ErrorFrameStyle BackColor="Tomato"></ErrorFrameStyle>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="QR Code" VerticalAlign="Middle">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="qrCodeTextBox" runat="server" Width="100%" OnTextChanged="customTextChanged" AutoPostBack="true" AutoCompleteType="Disabled"  />
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="" VerticalAlign="Middle" ColumnSpan="2">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="2" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView ID="GridView1" runat="server"  KeyFieldName="DESCRIZIONE_CONTENITORE" OnRowUpdating="GridView1_RowUpdating" DataSourceID="RegistrazioneContenitori" >
                                        <SettingsPager Visible="False">
                                        </SettingsPager>
                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" />
                                        <SettingsPopup>
                                            <FilterControl AutoUpdatePosition="False"></FilterControl>
                                        </SettingsPopup>
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowEditButton="True" ShowInCustomizationForm="True" VisibleIndex="0">
                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn FieldName="DESCRIZIONE_CONTENITORE" VisibleIndex="1" ReadOnly="True" Caption="DESCRIZIONE">
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataSpinEditColumn FieldName="QUANTITA_CONTENITORE" ShowInCustomizationForm="True" VisibleIndex="2" Caption="QUANTITA">
                                                <PropertiesSpinEdit DisplayFormatString="g" LargeIncrement="1" MaxValue="10000" MinValue="1">
                                                </PropertiesSpinEdit>
                                            </dx:GridViewDataSpinEditColumn>
                                        </Columns>
                                    </dx:ASPxGridView>
                                    <asp:SqlDataSource ID="RegistrazioneContenitori" runat="server" ConnectionString="<%$ ConnectionStrings:ContenitoriXCM %>" SelectCommand="SELECT [DESCRIZIONE_CONTENITORE], [QUANTITA_CONTENITORE] FROM [RC_TEMPORANEA] WHERE ([ID_DOCUMENTO] = @ID_DOCUMENTO)">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="ID_DOCUMENTO" SessionField="idDocumento" Type="Int64" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Larghezza" VerticalAlign="Middle" ColumnSpan="2">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="larghezzaTextBox" runat="server" Width="100%" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="customTextChanged">
                                        <%--<MaskSettings UseInvariantCultureDecimalSymbolOnClient="True" AllowMouseWheel="False" IncludeLiterals="None" Mask="###.##" PromptChar=" "/>--%>

                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="Text">
                                            <ErrorFrameStyle BackColor="Tomato"></ErrorFrameStyle>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Altezza" VerticalAlign="Middle" ColumnSpan="2">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="altezzaTextBox" runat="server" Width="100%" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="customTextChanged">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="Text">
                                            <ErrorFrameStyle BackColor="Tomato"></ErrorFrameStyle>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Profondità" VerticalAlign="Middle" ColumnSpan="2">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="profonditaTextBox" runat="server" Width="100%" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="customTextChanged">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="Text">
                                            <ErrorFrameStyle BackColor="Tomato"></ErrorFrameStyle>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem Caption="Peso" VerticalAlign="Middle" ColumnSpan="2">
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="pesoTextBox" runat="server" Width="100%" AutoCompleteType="Disabled" AutoPostBack="true" OnTextChanged="customTextChanged">
                                        <ValidationSettings RequiredField-IsRequired="true" Display="Dynamic" ErrorDisplayMode="Text">
                                            <ErrorFrameStyle BackColor="Tomato"></ErrorFrameStyle>

                                            <RequiredField IsRequired="True"></RequiredField>
                                        </ValidationSettings>
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" VerticalAlign="Middle" Paddings-PaddingTop="20px" CssClass="float-left">
                            <Paddings PaddingTop="20px"></Paddings>
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton runat="server" ID="saveButton" Text="Save" AutoPostBack="false" UseSubmitBehavior="False" OnClick="saveButton_Click">
                                        <ClientSideEvents Click="checkGridView" />
                                    </dx:ASPxButton>

                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" VerticalAlign="Middle" Paddings-PaddingTop="20px" CssClass="float-left">
                            <Paddings PaddingTop="20px"></Paddings>
                            <SpanRules>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="S"></dx:SpanRule>
                                <dx:SpanRule ColumnSpan="1" RowSpan="1" BreakpointName="M"></dx:SpanRule>
                            </SpanRules>
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton runat="server" ID="resetButton" Text="Reset" AutoPostBack="false" UseSubmitBehavior="False">
                                        <ClientSideEvents Click="function(s, e) { popupControl.Show(); }" />
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
    </div>

    <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="popupControl"
        Height="51px" Modal="True" CloseAction="CloseButton" Width="367px" AllowDragging="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowHeader="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                Sicuro di voler cancellare tutti i riferimenti a questo documento?
                <br />
                <br />
                <br />
                <br />
                <table style="border-style: none; border-color: inherit; border-width: thin; width: 337px;">
                    <tr style="align-content: center; text-align: center">
                        <td>
                            <dx:ASPxButton ID="btnOK" runat="server" AutoPostBack="False" Text="OK"
                                OnClick="resetButton_Click" UseSubmitBehavior="false" CausesValidation="false">
                                <ClientSideEvents Click="function(s, e) { popupControl.Hide();}" />

                                <Paddings PaddingRight="10px"></Paddings>
                            </dx:ASPxButton>

                        </td>   
                        <td>
                            <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" ClientInstanceName="btnCancel"
                                Text="Cancel" UseSubmitBehavior="false">
                                <ClientSideEvents Click="function(s, e) { popupControl.Hide(); }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>



