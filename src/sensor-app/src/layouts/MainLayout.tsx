import type { PropsWithChildren } from 'react';

function MainLayout({ children }: PropsWithChildren) {
  return (
    <div className="main-layout">
      <main>{children}</main>
    </div>
  );
}

export default MainLayout;
