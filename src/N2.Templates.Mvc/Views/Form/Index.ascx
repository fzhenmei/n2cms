<%@ Control Language="C#" AutoEventWireup="true" Inherits="N2.Web.Mvc.N2ModelViewUserControl<FormModel, Form>" %>
<%@ Import Namespace="System.Resources"%>
<div class="uc">
	<%if(Model.FormSubmitted){%>
		<%=ContentHtml.DisplayContent(m => m.SubmitText)%>
	<%}else{%>
		<%using(Html.BeginForm<FormController>(c => c.Submit(null), FormMethod.Post)){%>
			<%=ContentHtml.DisplayContent(m => m.Title)%>
			<%=ContentHtml.DisplayContent(m => m.IntroText)%>

			<div class="inputForm">
				<%foreach(var formElement in Model.Elements){%>
					<div class="row cf">
						<label class="label" for="<%=formElement.ElementID %>"><%=formElement.QuestionText%></label>
						<%=formElement.CreateHtmlElement()%>
					</div>
				<%}%>
			</div>

			<%=Html.SubmitButton("Submit", "Send")%>
		<%}%>
	<%}%>
</div>