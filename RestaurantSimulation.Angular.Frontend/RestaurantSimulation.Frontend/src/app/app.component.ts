import { DOCUMENT } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';
import { TestAccessTokenService } from './test-access-token.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'RestaurantSimulation.Frontend';

  constructor(
    @Inject(DOCUMENT) public document: Document,
    public auth: AuthService,
    private testAccessTokenService: TestAccessTokenService) {

  }

  getTestList() {
    this.testAccessTokenService.testApi().subscribe((list: string[]) => {
      console.log(list);
    })
  }

  getAccessToken() {
    console.log(this.auth.getAccessTokenSilently({ ignoreCache: true }).subscribe((token) => {
      console.log(token);
    }));
  }

  getRoles() {
    console.log(this.auth.getIdTokenClaims().subscribe((token) => {
      console.log(token);
    }));
  }

}
