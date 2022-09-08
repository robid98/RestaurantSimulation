import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TestAccessTokenService {

  constructor(private httpClient: HttpClient) {

  }

  public testApi(): Observable<string[]> {
    return this.httpClient.get<string[]>('https://localhost:7242/api/test/list')
  }
}
