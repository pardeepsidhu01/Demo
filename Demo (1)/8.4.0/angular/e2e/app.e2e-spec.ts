import { DemoTemplatePage } from './app.po';

describe('Demo App', function() {
  let page: DemoTemplatePage;

  beforeEach(() => {
    page = new DemoTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
