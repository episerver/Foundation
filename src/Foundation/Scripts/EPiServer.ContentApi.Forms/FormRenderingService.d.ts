declare module "EPiServer.ContentApi.Forms/FormRenderingService" {
	interface ContentApiFormModelAssets {
		[key: string]: string
	}

	export interface ContentApiFormModel {
		template: string
		assets: ContentApiFormModelAssets
	}

	export function render(formModel: ContentApiFormModel, attachNode: string | HTMLElement): void
}