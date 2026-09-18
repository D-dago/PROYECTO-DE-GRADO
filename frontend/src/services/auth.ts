export function getTokenRole(token: string): string {
  try {
    const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))) as Record<string, unknown>
    return (payload.role ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']) as string
  } catch {
    return ''
  }
}
