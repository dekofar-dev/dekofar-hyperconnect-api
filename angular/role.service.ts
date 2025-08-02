import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class RoleService {
  private readonly baseUrl = '/api';

  constructor(private http: HttpClient) {}

  getRoles(): Observable<string[]> {
    return this.http.get<string[]>(`${this.baseUrl}/roles`);
  }

  createRole(roleName: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/roles`, { roleName });
  }

  assignRoles(userId: string, roles: string[]): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/users/${userId}/roles`, { roles });
  }
}
